//TODO: Videos
//TODO: Use https://github.com/mudler/LocalAI to sort items, to make an 'algorithm'
//TODO: Reserve user names, to make sure that they do not choose the same names as an endpoint
//TODO: Test error
//TODO: Add background image to error page
//TODO: Change all root to view in all view partials

using App.Controllers;
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
builder.Services.AddTransient<FeedController>();

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
Service.Initialize(app.Services);

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
