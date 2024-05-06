using System.ComponentModel.DataAnnotations;
using App.Models.Entities;

namespace App.Models;

public class User
{
    [Required] public string DisplayName { get; set; } = null!;
    [Required] public string ImageUrl { get; set; } = null!;

    public static explicit operator User(UserEntity entity) =>
        new()
        {
            DisplayName = entity.UserName!,
            ImageUrl = $"/image/{entity.ImageId}"
        };
}
