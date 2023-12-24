namespace CommentForProductsDesktop.Models;

public class Comment
{
    public int Id { get; set; }

    public string? TextComment { get; set; }

    public byte[]? Photo { get; set; }

    public int? Score { get; set; }

    public int? IdUser { get; set; }

    public int? IdProduct { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}