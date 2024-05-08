using App.Models;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View(new HomeViewModel(initialRequestUrl: Endpoints.Feed.VideoRecommendations));
    }
}
