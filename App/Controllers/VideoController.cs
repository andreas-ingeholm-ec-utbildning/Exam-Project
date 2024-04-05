using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class VideosController : Controller
{
    public IActionResult Search([FromQuery] string? q)
    {
        //TODO: Return using htmx
        return StatusCode(200);
    }

    public IActionResult Home()
    {
        return StatusCode(200);
    }

    [Authorize]
    public IActionResult Subscriptions()
    {
        return StatusCode(200);
    }
}
