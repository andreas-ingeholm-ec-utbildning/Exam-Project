using System.ComponentModel.DataAnnotations;

namespace App.Models.Entities;

public class UserEntity : Entity
{
    [Required] public string Name { get; set; } = null!;
    [Required] public string ImageId { get; set; } = null!;
}
