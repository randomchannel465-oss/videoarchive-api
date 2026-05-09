using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

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