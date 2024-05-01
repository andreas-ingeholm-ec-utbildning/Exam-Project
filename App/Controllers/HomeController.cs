using System.Diagnostics;
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

    public IActionResult Index2(HomeViewModel viewModel)
    {
        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
