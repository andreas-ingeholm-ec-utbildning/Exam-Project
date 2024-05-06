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
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTailwindCssTagHelpers(builder.Configuration);

builder.Services.AddDbContext<DBContext>();
builder.Services.AddTransient<EntityService<UserEntity>>();
builder.Services.AddTransient<EntityService<VideoEntity>>();

builder.Services.AddIdentity<UserEntity, IdentityRole<Guid>>().AddEntityFrameworkStores<DBContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    options.User.RequireUniqueEmail = true;
});

builder.Services.AddCors(options =>
     {
         options.AddPolicy("AllowAll",
             builder =>
             {
                 builder
                 .AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials();
             });
     });

var app = builder.Build();

app.UseExceptionHandler("/Shared/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapHtmxAntiforgeryScript();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
