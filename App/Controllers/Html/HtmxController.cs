using App.Models;
using App.Models.Entities;
using App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class HtmxController : Controller, IHtmlResult
{
    /// <summary>Returns <paramref name="html"/> as response.</summary>
    public IActionResult Html(string html) =>
        new ContentResult() { Content = html };

    /// <summary>Called right before response is sent. Use this to set title or wrap partials, or similar.</summary>
    public virtual void OnHTMLResponse(HtmlResult html)
    { }

    HtmlResult? m_result;

    public HtmlResult HtmlResult => m_result ??= new(this);

    public IActionResult GeneratedHtml()
    {
        return IsHtmxRequest()
            ? HtmlResult
            : RedirectToHome();
    }

    public bool IsHtmxRequest()
    {
        return Request is not null && Request.Headers["hx-request"] == "true";
    }

    public ViewResult RedirectToHome(string? redirectUrl = null)
    {
        redirectUrl ??= ($"{Request?.Path}{Request?.QueryString}") ?? Endpoints.Feed.Recommendations;
        //User navigated to endpoint directly, lets redirect to page proper, then make request once more
        return View("~/Views/Home/Index.cshtml", new HomeViewModel(redirectUrl));
    }

    #region User

    public bool IsAuthenticated()
    {
        return User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<UserEntity?> GetUserAsync()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var entity = await Service.Get<UserManager<UserEntity>>().FindByNameAsync(User.Identity!.Name!);
            if (entity is null)
            {
                //Something is wrong, we can't find user, let's sign out whatever asp.net has logged in
                await Service.Get<SignInManager<UserEntity>>().SignOutAsync();
                return null;
            }

            return entity;
        }

        return null;
    }

    #endregion
    #region Error

    public IActionResult Error(string title, string message)
    {
        ClearHtml();
        SetBackground(Partials.Backgrounds.Error);
        SetTitle(title);
        AddPartial(Partials.Views.Error, message);
        return HtmlResult;
    }

    #endregion
    #region IHtmlResult

    public IHtmlResult AddPartial(string name) => HtmlResult.AddPartial(name);
    public IHtmlResult AddPartial<TModel>(string name, TModel model) => HtmlResult.AddPartial(name, model);
    public IHtmlResult AddPartials<TModel>(string name, params TModel[] model) => HtmlResult.AddPartials(name, model);
    public IHtmlResult AddPartials<TModel>(string name, string containerTailwindClassNames, params TModel[] model) => HtmlResult.AddPartials(name, model);
    public IHtmlResult SetBackground(string background) => HtmlResult.SetBackground(background);
    public IHtmlResult SetTitle(string title) => HtmlResult.SetTitle(title);
    public IHtmlResult WrapIn(string partial) => HtmlResult.WrapIn(partial);
    public IHtmlResult ClearHtml() => HtmlResult.ClearHtml();

    #endregion
}
