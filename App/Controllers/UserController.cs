using App.DB;
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
    public IActionResult Bookmarks()
    {
        if (!IsAuthenticated())
            return Login(redirectUrl: Endpoints.User.Bookmarks);

        SetTitle("Bookmarks - Youtube clone");
        SetBackground(Partials.Backgrounds.Bookmark);
        AddPartial(Partials.Views.Bookmarks);
        return GeneratedHtml();
    }

    [HttpGet(Endpoints.User.Upload)]
    public IActionResult Upload()
    {
        if (!IsAuthenticated())
            return Login(redirectUrl: Endpoints.User.Upload);

        SetTitle("Upload - Youtube clone");
        SetBackground(Partials.Backgrounds.Upload);
        AddPartial(Partials.Views.Upload);
        return GeneratedHtml();
    }

    #region User

    [HttpGet(Endpoints.User.Me)]
    [HttpGet("/user")]
    public async Task<IActionResult> Me()
    {
        if (!IsAuthenticated())
            return Login();

        var user = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (user is null)
        {
            await signInManager.SignOutAsync();
            ModelState.AddModelError("", "Something went wrong, please try log in again.");
            return Login();
        }

        return await UserVideos(user.UserName);
    }

    [HttpGet("/{user:alpha}")]
    [HttpGet("/{user:alpha}/videos")]
    public async Task<IActionResult> UserVideos(string? user, [FromQuery] int page = 0)
    {
        var entity = await userManager.FindByNameAsync(user ?? "");
        if (entity is null)
            return Error("No such user found.", "No such user found.");

        if (!IsHtmxRequest())
            return RedirectToHome();

        SetTitle($"{entity.UserName!} - Videos - Youtube clone");
        SetBackground(Partials.Backgrounds.Video);
        WrapIn(Partials.Part.Feed);

        AddPartial(Partials.Part.UserPageHeader, (User)entity);
        AddPartial(Partials.Part.TailwindContainer, "mt-44");
        AddPartials(Partials.Item.Video, Enumerable.Range(1, 50).Select(feedController.GetVideo).ToArray());

        return GeneratedHtml();
    }

    [HttpGet("/{user:alpha}/comments")]
    public async Task<IActionResult> UserComments(string? user)
    {
        var entity = await userManager.FindByNameAsync(user ?? "");
        if (entity is null)
            return Error("No such user found.", "No such user found.");

        if (!IsHtmxRequest())
            return RedirectToHome();

        SetTitle($"{entity.UserName!} - Comments - Youtube clone");
        SetBackground(Partials.Backgrounds.Comment);
        WrapIn(Partials.Part.Feed);

        AddPartial(Partials.Part.UserPageHeader, (User)entity);

        return GeneratedHtml();
    }

    [HttpGet(Endpoints.User.Edit)]
    public async Task<IActionResult> Edit()
    {
        if (!IsAuthenticated())
            return Login(redirectUrl: Endpoints.User.Upload);

        var entity = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (entity is null)
            return Error("No such user found.", "No such user found.");

        var user = (User)entity;

        SetTitle("Edit - Youtube clone");
        SetBackground(Partials.Backgrounds.EditUser);
        AddPartial(Partials.Views.EditUser, new EditUserViewModel(user));
        return GeneratedHtml();
    }

    [HttpPost(Endpoints.User.Edit)]
    public async Task<IActionResult> Edit(EditUserViewModel viewModel)
    {

        return Ok();

    }

    #endregion
    #region Login / Logout

    [HttpGet(Endpoints.User.Login)]
    public IActionResult Login(string? redirectUrl = null)
    {
        SetTitle("Login - Youtube clone");
        SetBackground(Partials.Backgrounds.LoginUser);
        AddPartial(Partials.Views.LoginUser, new LoginUserViewModel() { RedirectUrl = redirectUrl });
        return GeneratedHtml();
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
        AddPartial(Partials.Views.LoginUser, viewModel);

        SetTitle("Login - Youtube clone");
        SetBackground(Partials.Backgrounds.LoginUser);
        return GeneratedHtml();
    }

    [HttpPost(Endpoints.User.Logout)]
    [HttpGet(Endpoints.User.Logout)] //User could navigate here manually
    public async Task<IActionResult> Logout()
    {
        if (!IsHtmxRequest() || Request.Method != "POST")
            return RedirectToHome(string.Empty);

        await signInManager.SignOutAsync();
        return feedController.Recommended();
    }

    #endregion
    #region Create

    [HttpGet(Endpoints.User.Create)]
    public IActionResult Create()
    {
        SetTitle("Create user - Youtube clone");
        SetBackground(Partials.Backgrounds.CreateUser);
        AddPartial(Partials.Views.CreateUser);
        return GeneratedHtml();
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

        SetTitle("Create user - Youtube clone");
        SetBackground(Partials.Backgrounds.CreateUser);

        if (!ModelState.IsValid)
            return AddPartial(Partials.Views.CreateUser, viewModel).GeneratedHtml();

        var entity = new UserEntity() { Email = viewModel.EmailAddress, UserName = viewModel.DisplayName! };

        var result = await userManager.CreateAsync(entity, viewModel.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return AddPartial(Partials.Views.CreateUser, viewModel).GeneratedHtml();
        }

        await dbContext.SaveChangesAsync();
        await signInManager.PasswordSignInAsync(entity, viewModel.Password, viewModel.RememberMe, false);

        AddPartial(Partials.Views.User, (User)entity);

        return GeneratedHtml();
    }

    #endregion
}
