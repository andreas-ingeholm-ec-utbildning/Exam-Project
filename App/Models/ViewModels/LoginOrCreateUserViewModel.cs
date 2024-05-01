using System.ComponentModel.DataAnnotations;

namespace App.Models.ViewModels;

public class LoginOrCreateUserViewModel
{
    [Display(Name = "Display Name")]
    public string? DisplayName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string EmailAddress { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;
}