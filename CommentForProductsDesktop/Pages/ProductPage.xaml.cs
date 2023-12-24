using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using CommentForProductsDesktop.Models;

namespace CommentForProductsDesktop.Pages;

public partial class ProductPage : Page
{
    private Product _product;
    private User _user;
    private TokenRequest _token;
    public ProductPage()
    {
        InitializeComponent();
    }
    public ProductPage(Product product, User user, TokenRequest token)
    {
        InitializeComponent();
        _product = product;
        _token = token;
        _user = user;
        if (_user.IdRole==1)
        {
            AddCommentButton.Visibility = Visibility.Visible;
        }
        else
        {
            ViewAnalyticButton.Visibility = Visibility.Visible;
        }
    }

    private void AddCommentButton_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new AddCommentPage(_product,_user,_token));
    }

    private void ViewAnalyticButton_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new AnalyticsProductPage(_product,_token));
    }

    private async void ProductPage_OnLoaded(object sender, RoutedEventArgs e)
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
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        for (int i = 0; i < comments.Count; i++)
        {
            WebClient wc1 = new WebClient();
            string url1 = $"https://localhost:7173/api/Users/{comments[i].IdUser}";
            var json1 = wc.DownloadString(url1);
            User user = JsonSerializer.Deserialize<User>(json1,new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
            comments[i].User = user;
        }
        CommentListView.ItemsSource = comments.Where(c=>c.IdProduct==_product.Id);
    }
}