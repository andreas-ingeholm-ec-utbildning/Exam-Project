using Microsoft.AspNetCore.Identity;

namespace App.Models.Entities;

public class UserEntity : IdentityUser<Guid>, IEntity
{
    public string? ImageId { get; set; }

    public ICollection<CommentEntity> Comments { get; set; } = null!;
}
