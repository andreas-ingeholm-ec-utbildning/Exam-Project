using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace App.Controllers;

public class VideosController : Controller
{
    public async Task<IActionResult> Search([FromQuery] string? q)
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

        var html = await Task.WhenAll(videos.Select(v => this.RenderViewToStringAsync("_VideoItem", v)));
        return Content(string.Join("\n", html));
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
public static class ControllerExtensions
{
    /// <summary>
    /// Render a partial view to string.
    /// </summary>
    public static async Task<string> RenderViewToStringAsync(this Controller controller, string viewNamePath, object model = null)
    {
        if (string.IsNullOrEmpty(viewNamePath))
            viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

        controller.ViewData.Model = model;

        using (StringWriter writer = new StringWriter())
        {
            try
            {
                var view = FindView(controller, viewNamePath);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    view,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await view.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
            catch (Exception exc)
            {
                return $"Failed - {exc.Message}";
            }
        }
    }

    private static IView FindView(Controller controller, string viewNamePath)
    {
        IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

        ViewEngineResult viewResult = null;

        if (viewNamePath.EndsWith(".cshtml"))
            viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
        else
            viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

        if (!viewResult.Success)
        {
            var endPointDisplay = controller.HttpContext.GetEndpoint().DisplayName;

            if (endPointDisplay.Contains(".Areas."))
            {
                //search in Areas
                var areaName = endPointDisplay.Substring(endPointDisplay.IndexOf(".Areas.") + ".Areas.".Length);
                areaName = areaName.Substring(0, areaName.IndexOf(".Controllers."));

                viewNamePath = $"~/Areas/{areaName}/views/{controller.HttpContext.Request.RouteValues["controller"]}/{controller.HttpContext.Request.RouteValues["action"]}.cshtml";

                viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
            }

            if (!viewResult.Success)
                throw new Exception($"A view with the name '{viewNamePath}' could not be found");

        }

        return viewResult.View;
    }

}