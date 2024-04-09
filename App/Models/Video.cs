using System.ComponentModel.DataAnnotations;

namespace App.Models;

public class Video
{
    [Required] public string Id { get; init; } = null!;
    [Required] public User User { get; set; } = null!;

    [Required] public string Title { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [Required] public string ImageUrl { get; set; } = null!;
    [Required] public string WatchUrl { get; set; } = null!;
    [Required] public DateTime UploadDate { get; set; }
}