using System;
using System.Collections.Generic;

namespace CommentForProductApi.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Patronymic { get; set; }

    public string? Password { get; set; }

    public string? Login { get; set; }

    public int? IdRole { get; set; }
    public int? IdRefreshToken { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Role? IdRoleNavigation { get; set; }
    public virtual RefreshToken? IdRefreshTokenNavigation { get; set; }
}
