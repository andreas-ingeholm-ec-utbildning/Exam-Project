using App.DB;
using App.Models;
using App.Models.Entities;
using App.Models.ViewModels;
using App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class UserController(FeedController feedController, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, DBContext dbContext, IWebHostEnvironment environment, IMediaService mediaService) : HtmxController
{
    [HttpGet("/{user:alpha}")]
    [HttpGet("/{user:alpha}/videos")]
    public async Task<IActionResult> UserVideos(string? user)
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
        AddPartial(Partials.Part.TailwindContainer, "mt-52");
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
        if (!IsHtmxRequest())
            return RedirectToHome(string.Empty);

        await signInManager.SignOutAsync();
        return Redirect("/");
    }

    #endregion
    #region Logged in user

    [HttpGet(Endpoints.User.Me)]
    [HttpGet("/user")]
    public async Task<IActionResult> Me()
    {
        if (!IsAuthenticated())
            return Login();

        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            await signInManager.SignOutAsync();
            ModelState.AddModelError("", "Something went wrong, please try log in again.");
            return Login();
        }

        return await UserVideos(user.UserName);
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
        AddPartial(Partials.Views.EditUser, new EditUserViewModel() { DisplayName = user.DisplayName == entity.Email ? string.Empty : user.DisplayName, ImageUrl = user.ImageUrl });
        return GeneratedHtml();
    }

    [HttpPost(Endpoints.User.Edit)]
    public async Task<IActionResult> Edit(EditUserViewModel viewModel)
    {
        var isAuthenticated = IsAuthenticated();
        var entity = await userManager.FindByNameAsync(User.Identity!.Name!);
        var isChangingDisplayName = !string.IsNullOrEmpty(viewModel.DisplayName) && entity?.UserName != viewModel.DisplayName;
        var isNameDuplicate = isChangingDisplayName && userManager.FindByNameAsync(viewModel.DisplayName!) is null;

        if (!ModelState.IsValid || !isAuthenticated || entity is null || isNameDuplicate)
        {
            if (ModelState.IsValid)
            {
                if (!isAuthenticated)
                    ModelState.AddModelError("", "Could not authorize.");

                if (entity is null)
                    ModelState.AddModelError("", "Could not find user, please try again.");

                if (isNameDuplicate)
                    ModelState.AddModelError("", "The display name is already in use.");
            }

            SetTitle("Edit - Youtube clone");
            SetBackground(Partials.Backgrounds.EditUser);
            AddPartial(Partials.Views.EditUser, viewModel);
            return GeneratedHtml();
        }
        else
        {
            if (isChangingDisplayName)
                entity.UserName = viewModel.DisplayName;

            if (viewModel.Image is not null)
            {
                var id = await mediaService.Upload(viewModel.Image, MediaKind.Image);
                entity.ImageId = id;
            }

            await userManager.UpdateAsync(entity);
            await dbContext.SaveChangesAsync();
            await signInManager.RefreshSignInAsync(entity);

            return await Me();
        }
    }

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

        SetTitle("Create user - Youtube clone");
        SetBackground(Partials.Backgrounds.CreateUser);

        if (!ModelState.IsValid)
            return AddPartial(Partials.Views.CreateUser, viewModel).GeneratedHtml();

        var entity = new UserEntity() { Email = viewModel.EmailAddress, UserName = viewModel.EmailAddress!, ImageId = "default-avatar" };

        var result = await userManager.CreateAsync(entity, viewModel.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return AddPartial(Partials.Views.CreateUser, viewModel).GeneratedHtml();
        }

        await dbContext.SaveChangesAsync();
        await signInManager.PasswordSignInAsync(entity, viewModel.Password, viewModel.RememberMe, false);

        return Redirect(Endpoints.User.Edit);
    }

    #endregion
}
