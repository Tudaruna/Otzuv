using System.IO;
using System.Net;
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
using Type = CommentForProductsDesktop.Models.Type;

namespace CommentForProductsDesktop.Pages;

public partial class AddProductPage : Page
{
    private Product _product;
    private TokenRequest _token;
    public AddProductPage()
    {
        InitializeComponent();
        _product = new Product();
        TextBox nameBox = NameTextBox;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(nameBox, "Введите наименование");
        TextBox descriptionBox = DescriptionTextBox;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(descriptionBox, "Введите описание");
    }
    public AddProductPage(TokenRequest token)
    {
        InitializeComponent();
        _product = new Product();
        _token = token;
        TextBox nameBox = NameTextBox;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(nameBox, "Введите наименование");
        TextBox descriptionBox = DescriptionTextBox;
        MaterialDesignThemes.Wpf.HintAssist.SetHint(descriptionBox, "Введите описание");
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

    private async Task<string> AddProduct()
    {
        string jsonContent = JsonSerializer.Serialize<Product>(_product);
        //using StringContent jsonContent = new(JsonSerializer.Serialize<Client>(newclient), Encoding.UTF8, "application/json");
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Clear();
        client.BaseAddress = new Uri("https://localhost:7173/"); 
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
        
        var responseMessage = await client.PostAsync("api/Products",new StringContent(jsonContent,Encoding.UTF8,"application/json"));
        var contents = await responseMessage.Content.ReadAsStringAsync();
        return contents;
    }

    private async void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var text = await  AddProduct();
            _product = JsonSerializer.Deserialize<Product>(text.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (_product.Id != 0)
            {
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Ошибка создания продукта!");
            }
        }
        catch
        {
            var text = await RefreshToken();
            _token = JsonSerializer.Deserialize<TokenRequest>(text.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var text1 = await  AddProduct();
            _product = JsonSerializer.Deserialize<Product>(text1.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (_product.Id != 0)
            {
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Ошибка создания продукта!");
            }
        }
    }

    private void AddPhotoButton_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        if(openFileDialog.ShowDialog()==true)
        {
            openFileDialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";
            string fileName = openFileDialog.FileName;
            ProductImage.Source = new BitmapImage(new Uri(fileName));
            _product.Photo = File.ReadAllBytes(fileName);
        }
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
                    _product.Photo = File.ReadAllBytes(file);
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

    private async void AddProductPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.DataContext = _product;
        try
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("Authorization", "Bearer " + _token.Token);
            string url = $"https://localhost:7173/api/Types";
            var json = wc.DownloadString(url);
            List<Type> types = JsonSerializer.Deserialize<List<Type>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            TypeProductComboBox.ItemsSource = types;
        }
        catch
        {
            var text = await RefreshToken();
            _token = JsonSerializer.Deserialize<TokenRequest>(text.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            WebClient wc = new WebClient();
            wc.Headers.Add("Authorization", "Bearer " + _token.Token);
            string url = $"https://localhost:7173/api/Types";
            var json = wc.DownloadString(url);
            List<Type> types = JsonSerializer.Deserialize<List<Type>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            TypeProductComboBox.ItemsSource = types;
        }
    }

    private void TypeProductComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Type type = TypeProductComboBox.SelectedItem as Type;
        _product.IdType = type.Id;
    }
}