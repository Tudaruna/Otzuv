using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommentForProductsDesktop.Models;
using Microsoft.Win32;

namespace CommentForProductsDesktop.Pages;

public partial class AddCommentPage : Page
{
    private Product _product;
    private Comment _comment;
    private TokenRequest _token;
    private User _user;
    public AddCommentPage()
    {
        InitializeComponent();
    }
    public AddCommentPage(Product product, User user,TokenRequest token)
    {
        InitializeComponent();
        _product = product;
        _user = user;
        _token = token;
        _comment = new Comment();
        ScoreComboBox.SelectedIndex = 5;
        _comment.Score = 5;
        TextBox descriptionBox = Box;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(descriptionBox, "Введите комментарий");
    }

    private void AddPhotoButton_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        if(openFileDialog.ShowDialog()==true)
        {
            openFileDialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";
            string fileName = openFileDialog.FileName;
            ProductImage.Source = new BitmapImage(new Uri(fileName));
            _comment.Photo = File.ReadAllBytes(fileName);
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

    private void AddPhotoButton_OnDrop(object sender, DragEventArgs e)
    {
        try
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 1)
                MessageBox.Show("Нужно выбрать 1 файл!");
            else
            {
                string file = files[0];
                if (file.EndsWith(".png") || file.EndsWith(".jpg")|| file.EndsWith(".jpeg"))
                {
                    Image image = new Image();
                    image.Width = 150;
                    image.Height = 150;
                    image.Source = new BitmapImage(new Uri(file));
                    ProductImage.Source = new BitmapImage(new Uri(file));
                    _comment.Photo = File.ReadAllBytes(file);
                }
                else
                {
                    MessageBox.Show("Только картинки");
                }
            }

        }
        catch
        {
            MessageBox.Show("Неожиданная ошибка");
        }
    }
    private async Task<string> AddComment()
    {
        _comment.IdUser = _user.Id;
        _comment.IdProduct = _product.Id;
        string jsonContent = JsonSerializer.Serialize<Comment>(_comment);
        //using StringContent jsonContent = new(JsonSerializer.Serialize<Client>(newclient), Encoding.UTF8, "application/json");
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Clear();
        client.BaseAddress = new Uri("https://localhost:7173/"); 
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
        var responseMessage = await client.PostAsync("api/Comments",new StringContent(jsonContent,Encoding.UTF8,"application/json"));
        var contents = await responseMessage.Content.ReadAsStringAsync();
        return contents;
    }
    private async void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        
        try
        {
            var text = await AddComment();
            _comment = JsonSerializer.Deserialize<Comment>(text.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (_comment.Id != 0)
            {
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Ошибка создания комментария!");
            }
        }
        catch
        {
            var text = await RefreshToken();
            _token = JsonSerializer.Deserialize<TokenRequest>(text.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var text1 = await AddComment();
            _comment = JsonSerializer.Deserialize<Comment>(text1.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (_comment.Id != 0)
            {
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Ошибка создания комментария!");
            }
        }
    }

    private void ScoreComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _comment.Score = ScoreComboBox.SelectedIndex+1;
    }

    private void AddCommentPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.DataContext = _comment;
    }
}