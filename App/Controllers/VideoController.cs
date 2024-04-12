using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class VideosController : HtmxController
{
    public IActionResult Search([FromQuery] string? q)
    {
        var videos = Enumerable.Range(1, 50).Select(GetVideo).ToArray();

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

        return Html().AddPartials("_VideoItem", videos);
    }

    public IActionResult Home()
    {
        return StatusCode(200);
    }
}
