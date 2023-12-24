using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using CommentForProductsDesktop.Models;

namespace CommentForProductsDesktop.Pages;

public partial class RegPage : Page
{
    private User _user;
    public RegPage()
    {
        InitializeComponent();
        _user = new User();
        TextBox passwordBox = PasswordTextBox;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(passwordBox, "Введите пароль");
        TextBox nameBox = NameTextBox;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(nameBox, "Введите имя");
        TextBox lastNameBox = LastNameTextBox;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(lastNameBox, "Введите фамилию");
        TextBox patronymicBox = PatronymicTextBox;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(patronymicBox, "Введите отчество");
        TextBox loginBox = LoginTextBox;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(loginBox, "Введите логин");
    }

    private async Task<string> Registration()
    {
        _user.Password = PasswordTextBox.Text;
        string jsonContent = JsonSerializer.Serialize<User>(_user);
        //using StringContent jsonContent = new(JsonSerializer.Serialize<Client>(newclient), Encoding.UTF8, "application/json");
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Clear();
        client.BaseAddress = new Uri("https://localhost:7173/"); 
        
        var responseMessage = await client.PostAsync("api/AuthManagement/Register",new StringContent(jsonContent,Encoding.UTF8,"application/json"));
        var contents = await responseMessage.Content.ReadAsStringAsync();
        return contents;
    }
    
    private async void RegButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_user.Login != "" && _user.Password != "")
        {
            var text = await Registration();
            TokenRequest token = JsonSerializer.Deserialize<TokenRequest>(text.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (token != null)
                NavigationService.GoBack();
            else
                MessageBox.Show("Ошибка регистрации нового пользователя! Логин уже есть в системе!","Ошибка!");
        }
        else
        {
            MessageBox.Show("Ошибка регистрации нового пользователя! Не введен логин или пароль!","Ошибка!");
        }
    }

    private void RegPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.DataContext = _user;
    }
    
    public static string ComputeSHA256Hash(string text)
    {
        using (var sha256 = new SHA256Managed())
        {
            return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
        }                
    }
}