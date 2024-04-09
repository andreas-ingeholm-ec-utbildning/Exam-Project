using App.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class UserController(EntityService<UserEntity> userService) : Controller
{
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
