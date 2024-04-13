using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class VideosController : HtmxController
{
    public IActionResult Search([FromQuery] string? q)
    {
        var videos = Enumerable.Range(1, 50).Select(GetVideo);
        videos = videos.Where(v => v.Title.Contains(q ?? string.Empty, StringComparison.InvariantCultureIgnoreCase));
        return Html().AddPartials("_VideoItem", videos.ToArray());
    }

    public IActionResult Feed()
    {
        var videos = Enumerable.Range(1, 50).Select(GetVideo);
        return Html().AddPartials("_VideoItem", videos.ToArray());
    }

    Video GetVideo(int count)
    {
        return new()
        {
            Id = "test" + count,
            Title = "test video " + count,
            Description = "this is a test video " + count,
            ImageUrl = "https://i.ytimg.com/vi/gIRilYj3Tg4/hqdefault.jpg?sqp=-oaymwEcCNACELwBSFXyq4qpAw4IARUAAIhCGAFwAcABBg==&rs=AOn4CLDWF5fLqJZLdCZzSBCrykcOT_agAQ",
            UploadDate = DateTime.Now,
            WatchUrl = "/watch/test" + count,
        };
    }

}
