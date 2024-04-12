using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace App.Controllers;

public class HtmxController : Controller
{

    /// <summary>Returns <paramref name="html"/> as response.</summary>
    public IActionResult Html(string html) =>
        new ContentResult() { Content = html };

    /// <summary>Returns html content as response.</summary>
    /// <remarks>See <see cref="HtmlResult.AddPartial{T}(string)"/>.</remarks>
    public HtmlResult Html() =>
        new(this);

    public class HtmlResult(Controller controller) : ContentResult
    {
        readonly List<(string partialName, object? model)> partials = [];

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.Headers["ContentType"] = "text/html";
            var sb = new StringBuilder();
            foreach (var (partialName, model) in partials)
                sb.AppendLine(await RenderViewToStringAsync(partialName, model));

            using var sw = new StreamWriter(response.Body, Encoding.UTF8);
            await sw.WriteAsync(sb.ToString());
            await sw.DisposeAsync();
        }

        /// <summary>Renders the partial as html and adds it to the response.</summary>
        public HtmlResult AddPartial<T>(string name)
        {
            partials.Add((name, null));
            return this;
        }

        /// <summary>Renders the model with the specified partial.</summary>
        public HtmlResult AddPartial<T>(string name, T model) =>
            AddPartials(name, model);

        /// <summary>Renders all models with the specified partial.</summary>
        public HtmlResult AddPartials<T>(string name, params T[] model)
        {
            foreach (var obj in model)
                partials.Add((name, obj));

            return this;
        }

        //Taken, with modifications, from https://stackoverflow.com/a/65462120/24282772
        async Task<string> RenderViewToStringAsync(string viewNamePath, object? model = null)
        {
            if (string.IsNullOrEmpty(viewNamePath))
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

            controller.ViewData.Model = model;

            using var writer = new StringWriter();

            try
            {
                var view = FindView(viewNamePath);
                var viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, writer, new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
            catch (Exception ex)
            {
                return $"Failed - {ex.Message}";
            }
        }

        IView FindView(string viewNamePath)
        {
            var viewEngine = (ICompositeViewEngine)controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine))!;

            var viewResult =
                viewNamePath.EndsWith(".cshtml")
                ? viewEngine.GetView(viewNamePath, viewNamePath, false)
                : viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

            if (!viewResult.Success)
            {
                var endPointDisplay = controller.HttpContext.GetEndpoint()!.DisplayName;

                if (endPointDisplay!.Contains(".Areas."))
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
}
