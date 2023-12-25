using Microsoft.Build.Framework;

namespace CommentForProductApi.Models.Dto.Requests;

public class UserRegistrationRequestDto
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Surname { get; set; }
    [Required]
    public string? Patronymic { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public string? Login { get; set; }
}