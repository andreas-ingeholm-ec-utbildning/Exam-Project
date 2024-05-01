using App.Models.Entities;
using App.Models.ViewModels;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class UserController(EntityService<UserEntity> userService) : HtmxController
{
    [HttpGet(Endpoints.User.Me)]
    public IActionResult Me()
    {
        if (User.Identity?.IsAuthenticated ?? false)
            return Html().AddPartial(Partials.Views.User);
        else
            return Html().AddPartial(Partials.Views.LoginUser, new LoginOrCreateUserViewModel());
    }

    [HttpGet(Endpoints.User.Login)]
    public IActionResult Login()
    {
        return Html().AddPartial(Partials.Views.LoginUser);
    }

    [HttpPost(Endpoints.User.Login)]
    public IActionResult Login(LoginOrCreateUserViewModel viewModel)
    {
        return Html().AddPartial(Partials.Views.LoginUser, viewModel);
    }

    [HttpGet(Endpoints.User.Create)]
    public IActionResult Create()
    {
        return Html().AddPartial(Partials.Views.CreateUser);
    }

    [HttpPost(Endpoints.User.Create)]
    public IActionResult Create(LoginOrCreateUserViewModel viewModel)
    {
        return Html().AddPartial(Partials.Views.CreateUser, viewModel);
    }

    public IActionResult Search([FromQuery] string? q)
    {
        return StatusCode(200);
    }

    public IActionResult Get([FromQuery] string? id)
    {
        //TODO: Return using htmx
        return StatusCode(200);
    }

    [HttpPost]
    public IActionResult Create([FromQuery] string? id)
    {
        //TODO: Return using htmx
        return StatusCode(200);
    }

    [Authorize]
    public IActionResult Update([FromQuery] string? id)
    {
        //TODO: Return using htmx
        return StatusCode(200);
    }

    [Authorize]
    public IActionResult Delete([FromQuery] string? id)
    {
        //TODO: Return using htmx
        return StatusCode(200);
    }
}
