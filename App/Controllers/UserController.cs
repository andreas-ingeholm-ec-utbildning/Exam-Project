using App.Models.Entities;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class UserController(EntityService<UserEntity> userService) : HtmxController
{
    [HttpGet(Endpoints.User.Me)]
    public IActionResult Me()
    {
        return Partial(true, Partials.Views.User);
    }

    [HttpGet(Endpoints.User.Bookmarks)]
    public IActionResult Bookmarks()
    {
        return Partial(true, Partials.Views.Bookmarks);
    }

    [HttpGet(Endpoints.User.Login)]
    public IActionResult Login()
    {
        return Partial(Partials.Views.LoginUser);
    }

    [HttpGet(Endpoints.User.Create)]
    public IActionResult Create()
    {
        return Partial(Partials.Views.CreateUser);
    }

    //[HttpPost(Endpoints.User.Login)]
    //public IActionResult Login(LoginOrCreateUserViewModel viewModel)
    //{
    //    return base.Ok();
    //}

    //[HttpPost(Endpoints.User.Create)]
    //public IActionResult Create(LoginOrCreateUserViewModel viewModel)
    //{
    //    return base.Ok();
    //}
}
