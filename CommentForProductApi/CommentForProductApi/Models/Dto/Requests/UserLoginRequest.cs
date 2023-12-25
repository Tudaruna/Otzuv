using Microsoft.Build.Framework;

namespace CommentForProductApi.Models.Dto.Requests;

public class UserLoginRequest
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}