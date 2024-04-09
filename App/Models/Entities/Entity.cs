using System.ComponentModel.DataAnnotations;

namespace App.Models.Entities;

public class Entity
{
    [Required] public string Id { get; set; } = null!;
}
