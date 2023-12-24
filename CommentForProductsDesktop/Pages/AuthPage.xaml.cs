using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using CommentForProductsDesktop.Models;

namespace CommentForProductsDesktop.Pages;

public partial class AuthPage : Page
{
    private User _user;
    
    public AuthPage()
    {
        InitializeComponent();
        _user = new User();
        TextBox nameBox = LoginTextBox;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(nameBox, "Введите логин");
        PasswordBox descriptionBox = PasswordTextBox;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(descriptionBox, "Введите пароль");
    }

    public async Task<string> Authorize()
    {
        AuthUser auth = new AuthUser()
        {
            Login = LoginTextBox.Text,
            Password = PasswordTextBox.Password
        };
        using StringContent jsonContent = new(
            JsonSerializer.Serialize<AuthUser>(auth), Encoding.UTF8, "application/json");
        
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7173/");
        var responseMessage = await client.PostAsync("api/AuthManagement/Login", jsonContent);
        
        var contents = await responseMessage.Content.ReadAsStringAsync();
        return contents;
    }
    

    private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var text = await Authorize();
            TokenRequest token = JsonSerializer.Deserialize<TokenRequest>(text.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var stream = token.Token;  
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == "Id").Value;
            
            WebClient wc = new WebClient();
            string url = $"https://localhost:7173/api/Users/{id}";
            var json = wc.DownloadString(url);
            _user = JsonSerializer.Deserialize<User>(json,new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
            
            
            if (_user.Id != 0)
            {
                NavigationService.Navigate(new ProductsPage(_user,token));
            }
            else
            {
                MessageBox.Show("Данного пользователя нет в системе!", "Ошибка авторизации");
            }
        }
        catch
        {
            MessageBox.Show("Ошибка подключения к API!","Ошибка");
        }
    }

    public static string ComputeSHA256Hash(string text)
    {
        using (var sha256 = new SHA256Managed())
        {
            return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
        }                
    }

    private void RegButton_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new RegPage());
    }
}