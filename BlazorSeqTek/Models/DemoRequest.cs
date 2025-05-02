using System.ComponentModel.DataAnnotations;

public class DemoRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name is too long")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company is required")]
    [StringLength(100, ErrorMessage = "Company name is too long")]
    public string Company { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}