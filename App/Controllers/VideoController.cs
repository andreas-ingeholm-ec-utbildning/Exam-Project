﻿using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class VideoController : HtmxController
{

    [Route("/video/{id?}")]
    public IActionResult Index(string id)
    {
        return Html().AddPartial(Partials.Views.Video);
    }

}