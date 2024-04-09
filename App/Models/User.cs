using System.ComponentModel.DataAnnotations;

namespace App.Models;

public class User
{
    [Required] public string Name { get; set; } = null!;
    [Required] public string ImageUrl { get; set; } = null!;
    [Required] public string UserUrl { get; set; } = null!;
}
