using System.ComponentModel.DataAnnotations;

namespace BlogpostService.Application.DTOs;

public class UserDto
{
    [Required(ErrorMessage = "The username must be supplied")]
    [MaxLength(length:255)]
    public string? Username { get; set; }
    
    [Required(ErrorMessage = "The first name must be supplied")]
    [MaxLength(length:255)]
    public string? FirstName { get; set; }
    
    [Required(ErrorMessage = "The last name must be supplied")]
    [MaxLength(length:255)]
    public string? LastName { get; set; }
}