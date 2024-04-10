using Microsoft.AspNetCore.Mvc;
using CloudWeather.Temperature.DataAccess;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TemperatureDbContext>( 
    opts => {
        opts.EnableSensitiveDataLogging();
       opts.EnableDetailedErrors();
       opts.UseNpgsql(builder.Configuration.GetConnectionString("AppDb")); 
    }, ServiceLifetime.Transient);

var app = builder.Build();

app.MapGet("/observation/{zip}", (string zip, [FromQuery] int? days, TemperatureDbContext db) =>{
    
    if(days == null || days < 1 || days > 30)
        return Results.BadRequest("Please provide a 'days' query parameter beteen 1 and 30");
    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
    var results = db.Temperature
    .Where( temp => temp.ZipCode == zip && temp.CreatedOn > startDate)
    .ToList();
    return Results.Ok(results);
});


app.Run();
