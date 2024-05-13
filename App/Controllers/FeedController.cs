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
    [Route(Endpoints.Feed.Recommendations)]
    public IActionResult Recommended([FromQuery] FeedKind kind = FeedKind.Video)
    {
        //TODO: Could we add chat gpt integration here? There seem to be some free alternatives / proxies that could work

        SetTitle("Youtube clone");
        WrapIn(Partials.Part.Feed);

        if (kind == FeedKind.Video)
            AddPartials(Partials.Item.Video, Enumerable.Range(1, 50).Select(GetVideo).ToArray());
        else if (kind == FeedKind.User)
            AddPartials(Partials.Item.User, Enumerable.Range(1, 50).Select(GetUser).ToArray());

        return GeneratedHtml();
    }

    [Route(Endpoints.Feed.Search)]
    public IActionResult Search([FromQuery] string? q = null, [FromQuery] FeedKind kind = FeedKind.Video)
    {
        if (string.IsNullOrEmpty(q))
            return Recommended(kind);

        SetTitle($"Searching for '{q.ToString().Trim()}' - Youtube clone");
        WrapIn(Partials.Part.Feed);

        if (kind == FeedKind.Video)
            AddPartial(Partials.Item.Video, Enumerable.Range(1, 50).Select(GetVideo).Search(q, v => v.Title).ToArray());
        else
            AddPartial(Partials.Item.User, Enumerable.Range(1, 50).Select(GetUser).Search(q, u => u.DisplayName).ToArray());

        return GeneratedHtml();
    }

    public Video GetVideo(int count)
    {
        return new()
        {
            Id = "test" + count,
            Title = "test video " + count,
            Description = "this is a test video " + count,
            ImageUrl = "https://i.ytimg.com/vi/gIRilYj3Tg4/hqdefault.jpg?sqp=-oaymwEcCNACELwBSFXyq4qpAw4IARUAAIhCGAFwAcABBg==&rs=AOn4CLDWF5fLqJZLdCZzSBCrykcOT_agAQ",
            UploadDate = DateTime.Now,
            WatchUrl = "/watch/test" + count,
            User = new() { DisplayName = "test userggggggggggggggggggggggggggggggggggggggggggggggggggggggg" }
        };
    }

    User GetUser(int count)
    {
        return new()
        {
            DisplayName = "test user " + count
        };
    }
}
