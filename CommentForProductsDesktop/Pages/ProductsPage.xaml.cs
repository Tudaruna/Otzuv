using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using CommentForProductsDesktop.Models;
using Type = CommentForProductsDesktop.Models.Type;

namespace CommentForProductsDesktop.Pages;

public partial class ProductsPage : Page
{
    private User _user;
    private Product _product;
    private TokenRequest _token;
    public ProductsPage()
    {
        InitializeComponent();
    }
    public ProductsPage(User user, TokenRequest token)
    {
        InitializeComponent();
        _user = user;
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


    private void GetProducts()
    {
        WebClient wc = new WebClient();
        wc.Headers.Add("Authorization", "Bearer " + _token.Token);
        string url = $"https://localhost:7173/api/Products";
        var json = wc.DownloadString(url);
        List<Product> products = JsonSerializer.Deserialize<List<Product>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        string urlType = $"https://localhost:7173/api/Types";
        var jsonType = wc.DownloadString(urlType);
        List<Type> types = JsonSerializer.Deserialize<List<Type>>(jsonType,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        for (int i = 0; i < products.Count; i++)
        {
            products[i].IdTypeNavigation = types.FirstOrDefault(c => c.Id == products[0].IdType);
        }

        ProductsListView.ItemsSource = products;
        if (_user.IdRole == 2)
        {
            AddButton.Visibility = Visibility.Visible;
        }
    }
    private async void ProductsPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            GetProducts();
        }
        catch
        {
            var text = await RefreshToken();
            _token = JsonSerializer.Deserialize<TokenRequest>(text.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            GetProducts();
        }
        
    }
    

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new AddProductPage(_token));
    }

    // private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    // {
    //     _product = e.Source as Product;
    //     
    //     //_product = ProductsListView.SelectedItem as Product;
    //     NavigationService.Navigate(new AddCommentPage(_product));
    // }
    private void ProductsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _product = ProductsListView.SelectedItem as Product;
        if (_product != null)
        {
            NavigationService.Navigate(new ProductPage(_product,_user,_token));
        }
    }

    // private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    // {
    //     try
    //     {
    //         GetProducts();
    //     }
    //     catch
    //     {
    //         var text = await RefreshToken();
    //         _token = JsonSerializer.Deserialize<TokenRequest>(text.ToString(),
    //             new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    //         GetProducts();
    //     }
    // }
}