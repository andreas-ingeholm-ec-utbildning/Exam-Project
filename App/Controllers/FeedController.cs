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
    public override void OnHTMLResponse(HtmlResult html)
    {
        if (Request.Query.TryGetValue("q", out var q))
        {
            ViewData["q"] = q;
            html.SetTitle($"Searching for '{q.ToString().Trim()}' - Youtube clone");
        }
        else
        {
            ViewData["q"] = q;
            html.SetTitle("Youtube clone");
        }
    }

    [Route(Endpoints.Feed.Search)]
    public IActionResult Search([FromQuery] string q, [FromQuery] FeedKind kind = FeedKind.Video)
    {
        return
            kind == FeedKind.Video
            ? Partial(Partials.Item.Video, Enumerable.Range(1, 50).Select(GetVideo).Search(q, v => v.Title).ToArray())
            : Partial(Partials.Item.User, Enumerable.Range(1, 50).Select(GetUser).Search(q, u => u.Name).ToArray());
    }

    [Route(Endpoints.Feed.Recommendations)]
    public IActionResult Recommended([FromQuery] FeedKind kind = FeedKind.Video)
    {
        return
            kind == FeedKind.Video
            ? Partial(Partials.Item.Video, Enumerable.Range(1, 50).Select(GetVideo).ToArray())
            : Partial(Partials.Item.User, Enumerable.Range(1, 50).Select(GetUser).ToArray());
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
            User = new() { Name = "test userggggggggggggggggggggggggggggggggggggggggggggggggggggggg" }
        };
    }

    User GetUser(int count)
    {
        return new()
        {
            Name = "test user " + count
        };
    }
}
