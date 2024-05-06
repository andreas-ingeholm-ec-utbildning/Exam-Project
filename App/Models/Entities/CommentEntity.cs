namespace App.Models.Entities;

public class CommentEntity : IEntity
{
    public Guid Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public Guid VideoId { get; set; }
    public VideoEntity Video { get; set; } = null!;
}