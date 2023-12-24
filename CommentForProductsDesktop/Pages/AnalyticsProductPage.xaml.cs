using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using CommentForProductsDesktop.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace CommentForProductsDesktop.Pages;

public partial class AnalyticsProductPage : Page
{
    Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
    private Product _product;
    private TokenRequest _token;
    private List<Comment> _comments;
    public AnalyticsProductPage()
    {
        InitializeComponent();
    }
    public AnalyticsProductPage(Product product,TokenRequest token)
    {
        InitializeComponent();
        _product = product;
        _token = token;
    }
    
    
    private async Task<string> RefreshToken()
    {
        using StringContent jsonContent = new(
            JsonSerializer.Serialize<TokenRequest>(_token), Encoding.UTF8, "application/json");
        
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7173/");
        var responseMessage = await client.PostAsync("api/AuthManagement/RefreshToken", jsonContent);
        
        var contents = await responseMessage.Content.ReadAsStringAsync();
        return contents;
    }


    private void GetComment()
    {
        WebClient wc = new WebClient();
        wc.Headers.Add("Authorization", "Bearer " + _token.Token);
        string url = $"https://localhost:7173/api/Comments";
        var json = wc.DownloadString(url);
        _comments = JsonSerializer.Deserialize<List<Comment>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        int sr = 0;
        ForDiagram forDiagrams = new ForDiagram();
        
        for (int i = 0; i < _comments.Count; i++)
        {
            if (_comments[i].IdProduct == _product.Id)
            {
                WebClient wc1 = new WebClient();
                string url1 = $"https://localhost:7173/api/Users/{_comments[i].IdUser}";
                var json1 = wc.DownloadString(url1);
                User user = JsonSerializer.Deserialize<User>(json1,new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
                _comments[i].User = user;
                sr += Convert.ToInt32(_comments[i].Score);
                if (_comments[i].Score == 5)
                    forDiagrams.Five += 1;
                else if (_comments[i].Score == 4)
                    forDiagrams.Four += 1;
                else if (_comments[i].Score == 3)
                    forDiagrams.Three += 1;
                else if (_comments[i].Score == 2)
                    forDiagrams.Two += 1;
                else if (_comments[i].Score == 1)
                    forDiagrams.One += 1;
            }
            
        }
        CommentListView.ItemsSource = _comments.Where(c=>c.IdProduct==_product.Id);
        SrScoreTextBlock.Text = $"{sr*1.0/_comments.Where(c=>c.IdProduct==_product.Id).Count()}";
        
        SeriesCollection ser = new SeriesCollection();
        PieSeries pie = new PieSeries
        {
            Title = "5",
            Values = new ChartValues<double> { forDiagrams.Five },
            DataLabels = true,
            LabelPoint = labelPoint,

        };
        PieSeries pie4 = new PieSeries
        {
            Title = "4",
            Values = new ChartValues<double> { forDiagrams.Four },
            DataLabels = true,
            LabelPoint = labelPoint,

        };
        PieSeries pie1 = new PieSeries
        {
            Title = "3",
            Values = new ChartValues<double> { forDiagrams.Three },
            DataLabels = true,
            LabelPoint = labelPoint,

        };
        PieSeries pie2 = new PieSeries
        {
            Title = "2",
            Values = new ChartValues<double> { forDiagrams.Two },
            DataLabels = true,
            LabelPoint = labelPoint,

        };
        PieSeries pie3 = new PieSeries
        {
            Title = "1",
            Values = new ChartValues<double> { forDiagrams.One },
            DataLabels = true,
            LabelPoint = labelPoint,

        };
        ser.Add(pie);
        ser.Add(pie1);
        ser.Add(pie2);
        ser.Add(pie3);
        ser.Add(pie4);

        pieChart.Series = ser;
    }

    private async void AnalyticsProductPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.DataContext = _product;
        try
        {
            GetComment();
        }
        catch
        {
            var text = await RefreshToken();
            _token = JsonSerializer.Deserialize<TokenRequest>(text.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            GetComment();
        }
    }
}