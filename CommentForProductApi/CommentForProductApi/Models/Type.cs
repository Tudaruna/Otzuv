using System;
using System.Collections.Generic;

namespace CommentForProductApi.Models;

public partial class Type
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
