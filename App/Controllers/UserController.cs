﻿using App.DB;
using App.Models;
using App.Models.Entities;
using App.Models.ViewModels;
using App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers;

public class UserController(FeedController feedController, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, DBContext dbContext) : HtmxController
{
    [HttpGet(Endpoints.User.Bookmarks)]
    public IActionResult Bookmarks() =>
        IsAuthenticated()
        ? Partial(Partials.Views.Bookmarks)
        : Partial(Partials.Views.LoginUser, new LoginUserViewModel() { RedirectUrl = Endpoints.User.Bookmarks });

    [HttpGet(Endpoints.User.Upload)]
    public IActionResult Upload() =>
        IsAuthenticated()
        ? Partial(Partials.Views.Upload)
        : Partial(Partials.Views.LoginUser, new LoginUserViewModel() { RedirectUrl = Endpoints.User.Upload });

    #region User

    [HttpGet(Endpoints.User.Me)]
    public async Task<IActionResult> Me()
    {
        if (!IsAuthenticated())
            return Partial(Partials.Views.LoginUser);

        var user = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (user is null)
        {
            await signInManager.SignOutAsync();
            ModelState.AddModelError("", "Something went wrong, please try log in again.");
            return Partial(Partials.Views.LoginUser, new LoginUserViewModel() { RedirectUrl = "/user/me" });
        }

        return await UserVideos(user.UserName);
    }

    [HttpGet("/{user:alpha}")]
    [HttpGet("/{user:alpha}/videos")]
    public async Task<IActionResult> UserVideos(string? user, [FromQuery] int page = 0)
    {
        var entity = await userManager.FindByNameAsync(user ?? "");
        if (entity is null)
            return Partial("Views/Error", $"No such user found.");

        var result = Partial("Part/_UserHeader", (User)entity);
        if (result is HtmlResult html)
        {
            html.AddPartials(Partials.Item.Video, Enumerable.Range(1, 50).Select(feedController.GetVideo).ToArray());
            html.WrapIn("<div class='row justify-content-center me-4 mt-44'>", "</div>");
            html.SetTitle($"{entity.UserName!} - Videos - Youtube clone");
            html.SetBackground("FontAwesome/_Video");
        }

        return result;
    }

    [HttpGet("/{user:alpha}/comments")]
    public async Task<IActionResult> UserComments(string? user, [FromQuery] int page = 0)
    {
        var entity = await userManager.FindByNameAsync(user ?? "");
        if (entity is null)
            return Partial("Views/Error", $"No such user found.");

        var result = Partial("Part/_UserHeader", (User)entity);
        if (result is HtmlResult html)
        {
            //html.AddPartials(Partials.Item.Video, Enumerable.Range(1, 50).Skip(page * 10).Take(10).Select(feedController.GetVideo).ToArray());
            html.SetTitle($"{entity.UserName!} - Comments - Youtube clone");
            html.SetBackground("FontAwesome/_Comment");
        }

        return result;
    }

    #endregion
    #region Login / Logout

    [HttpGet(Endpoints.User.Login)]
    public IActionResult Login()
    {
        return Partial(Partials.Views.LoginUser);
    }

    [HttpPost(Endpoints.User.Login)]
    public async Task<IActionResult> Login(LoginUserViewModel viewModel)
    {
        if (viewModel is not null && ModelState.IsValid)
        {
            if (await userManager.FindByEmailAsync(viewModel.EmailAddress) is UserEntity user)
            {
                var result = await signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, false);
                if (result.Succeeded)
                    return Redirect(viewModel.RedirectUrl ?? Endpoints.User.Me);
            }
        }

        ModelState.Clear();
        ModelState.AddModelError("", "Could not log in.");
        return Partial(Partials.Views.LoginUser, viewModel);
    }

    [HttpPost(Endpoints.User.Logout)]
    [HttpGet(Endpoints.User.Logout)] //User could navigate here manually
    public async Task<IActionResult> Logout()
    {
        if (!IsHtmxRequest() || Request.Method != "POST")
            return Redirect("/");

        await signInManager.SignOutAsync();
        return feedController.Recommended();
    }

    #endregion
    #region Create

    [HttpGet(Endpoints.User.Create)]
    public IActionResult Create()
    {
        return Partial(Partials.Views.CreateUser);
    }

    [HttpPost(Endpoints.User.Create)]
    public async Task<IActionResult> Create(CreateUserViewModel viewModel)
    {
        if (await userManager.FindByEmailAsync(viewModel.EmailAddress) is not null)
            ModelState.AddModelError("", "Email address already in use.");

        if (string.IsNullOrEmpty(viewModel.DisplayName))
            ModelState.AddModelError("", "Display name cannot be empty.");

        if (await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == viewModel.DisplayName) is not null)
            ModelState.AddModelError("", "Display name already in use.");

        if (!ModelState.IsValid)
            return Partial(Partials.Views.CreateUser, viewModel);

        var entity = new UserEntity() { Email = viewModel.EmailAddress, UserName = viewModel.DisplayName! };

        var result = await userManager.CreateAsync(entity, viewModel.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return Partial(Partials.Views.CreateUser, viewModel);
        }

        await dbContext.SaveChangesAsync();
        await signInManager.PasswordSignInAsync(entity, viewModel.Password, viewModel.RememberMe, false);

        return Partial(Partials.Views.User, (User)entity);
    }

    #endregion
}
