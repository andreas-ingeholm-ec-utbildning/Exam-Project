using System.ComponentModel.DataAnnotations;

namespace App.Models.ViewModels;

public class CreateUserViewModel
{
    const string EmailErrorMessage = "Email is invalid.";
    const string PasswordErrorMessage = "" +
        "Password must contain:\n" +
        "One lowercase letter\n" +
        "One uppercase letter\n" +
        "One digit\n" +
        "One special character.";

    [Required(ErrorMessage = EmailErrorMessage)]
    [EmailAddress(ErrorMessage = EmailErrorMessage)]
    [Display(Name = "Email Address")]
    public string EmailAddress { get; set; } = null!;

    [Required(ErrorMessage = PasswordErrorMessage)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = PasswordErrorMessage)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [Display(Name = "Display Name")]
    public string? DisplayName { get; set; }

    public bool RememberMe { get; set; }
}
