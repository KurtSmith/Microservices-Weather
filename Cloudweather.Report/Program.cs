using Microsoft.AspNetCore.Mvc;
using CloudWeather.Report.DataAccess;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ReportDbContext>( 
    opts => {
        opts.EnableSensitiveDataLogging();
       opts.EnableDetailedErrors();
       opts.UseNpgsql(builder.Configuration.GetConnectionString("AppDb")); 
    }, ServiceLifetime.Transient);

var app = builder.Build();

app.MapGet("/observation/{zip}", (string zip, [FromQuery] int? days, ReportDbContext db) =>{
    
    if(days == null || days < 1 || days > 30)
        return Results.BadRequest("Please provide a 'days' query parameter beteen 1 and 30");
    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
    var results = db.WeatherReport
    .Where( report => report.ZipCode == zip && report.CreatedOn > startDate)
    .ToList();
    return Results.Ok(results);
});


app.Run();
