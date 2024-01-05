using Microsoft.Extensions.Options;
using Zaliczenie.Controllers;
using Zaliczenie.Database;
using Zaliczenie.Models;
using Zaliczenie.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseStrings")
    );

builder.Services.AddSingleton<GeneralService<SongModel>>();
builder.Services.AddSingleton<GeneralService<MovieModel>>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();


// Required to run tests
public partial class Program { }