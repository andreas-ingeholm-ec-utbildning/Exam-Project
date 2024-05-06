using System.ComponentModel.DataAnnotations;

namespace App.Models.Entities;

public class VideoEntity : IEntity
{
    public Guid Id { get; set; }

    [Required] public UserEntity User { get; set; } = null!;

    [Required] public string Title { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [Required] public string ImageId { get; set; } = null!;
    [Required] public string MediaId { get; set; } = null!;

    public ICollection<CommentEntity> Comments { get; set; } = null!;
}
