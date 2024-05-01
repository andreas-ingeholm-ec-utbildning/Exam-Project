//TODO: Make 'overlays', feed view (home + search), user view, watch view

//TODO: Videos
//TODO: Use https://github.com/mudler/LocalAI to sort items, to make an 'algorithm'

//TODO: Users
//TODO: Implement users
//TODO: Implement create / login
//TODO: Implement user page

//TODO: Feed view
//TODO: 

//TODO: Make uris more user friendly (/user/<id of user>), but make it as a translation layer client side

using App.DB;
using App.Models.Entities;
using App.Services;
using Htmx.TagHelpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTailwindCssTagHelpers(builder.Configuration);

builder.Services.AddTransient<UserContext>();
builder.Services.AddTransient<EntityService<UserEntity>>();
builder.Services.AddTransient<EntityService<VideoEntity>>();

var app = builder.Build();

app.UseExceptionHandler("/Shared/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapHtmxAntiforgeryScript();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
