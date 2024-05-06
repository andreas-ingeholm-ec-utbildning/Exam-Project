using System.ComponentModel.DataAnnotations;

namespace App.Models.Entities;

public interface IEntity
{
    [Required] public Guid Id { get; set; }
}
