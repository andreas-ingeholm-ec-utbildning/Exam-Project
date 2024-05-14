using System.ComponentModel.DataAnnotations;

namespace App.Models.ViewModels;

public class EditUserViewModel
{
    [Display(Name = "Display Name")]
    public string? DisplayName { get; set; }

    public string? ImageUrl { get; set; }

    [Display(Name = "Display Name")]
    [DataType(DataType.Upload)]
    public IFormFile? Image { get; set; }
}
