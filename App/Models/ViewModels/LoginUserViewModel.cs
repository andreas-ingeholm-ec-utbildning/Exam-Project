using System.ComponentModel.DataAnnotations;

namespace App.Models.ViewModels;

public class LoginUserViewModel
{
    [Display(Name = "Email Address")]
    public string EmailAddress { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
    public string? RedirectUrl { get; set; }
}