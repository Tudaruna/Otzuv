using System.Text.Json;
using System.Text.Json.Serialization;

namespace CommentForProductsDesktop.Models;

public class Type
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

}