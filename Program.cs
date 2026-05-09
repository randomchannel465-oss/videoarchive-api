using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize =
        500_000_000;
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

string videosPath =
    Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot",
        "Videos");

string thumbnailsPath =
    Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot",
        "Thumbnails");

Directory.CreateDirectory(videosPath);

Directory.CreateDirectory(thumbnailsPath);

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider =
        new PhysicalFileProvider(videosPath),

    RequestPath = "/Videos",

    ServeUnknownFileTypes = true
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider =
        new PhysicalFileProvider(thumbnailsPath),

    RequestPath = "/Thumbnails",

    ServeUnknownFileTypes = true
});

app.MapControllers();

app.Run();