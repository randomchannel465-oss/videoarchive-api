using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "VideoArchive API",
            Version = "v1"
        });
});

var app = builder.Build();

app.UseSwagger(c =>
{
    c.RouteTemplate =
        "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "VideoArchive API V1");

    c.RoutePrefix = "swagger";
});

string root =
    Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot");

Directory.CreateDirectory(root);

Directory.CreateDirectory(
    Path.Combine(root, "Videos"));

Directory.CreateDirectory(
    Path.Combine(root, "Thumbnails"));

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider =
        new PhysicalFileProvider(root),

    RequestPath = ""
});

app.MapControllers();

var port =
    Environment.GetEnvironmentVariable("PORT")
    ?? "8080";

app.Run($"http://0.0.0.0:{port}");