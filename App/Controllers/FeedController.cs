using App.Models;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public enum FeedKind
{
    Video, User
}

public class FeedController : HtmxController
{
    [Route(Endpoints.Feed.Search)]
    public IActionResult Search([FromQuery] string? q, [FromQuery] FeedKind kind = FeedKind.Video)
    {
        var videos = Enumerable.Range(1, 50).Select(GetVideo);
        videos = videos.Where(v => v.Title.Contains(q ?? string.Empty, StringComparison.InvariantCultureIgnoreCase));
        return Html().AddPartials("_VideoItem", videos.ToArray());
    }

    [Route(Endpoints.Feed.Recommendations)]
    public IActionResult Recommended([FromQuery] FeedKind kind = FeedKind.Video)
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

public class VideoController : HtmxController
{

    [Route("/video/{id?}")]
    public IActionResult Index(string id)
    {
        return Html().AddPartial("_VideoView");
    }

}