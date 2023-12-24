namespace CommentForProductsDesktop.Models;

public class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public byte[]? Photo { get; set; }

    public int? IdType { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Type? IdTypeNavigation { get; set; }
}