using System.ComponentModel.DataAnnotations;

namespace App.Models.ViewModels;

public class EditUserViewModel(User user)
{
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [Display(Name = "Display Name")]
    public string DisplayName { get; set; } = user.DisplayName;

    public string ImageUrl { get; set; } = user.ImageUrl;

    [Display(Name = "Display Name")]
    [DataType(DataType.Upload)]
    public IFormFile? Image { get; set; }
}
