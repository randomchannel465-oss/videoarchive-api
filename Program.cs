using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot")),
    RequestPath = ""
});

app.MapControllers();

var port =
    Environment.GetEnvironmentVariable("PORT")
    ?? "8080";

app.Run($"http://0.0.0.0:{port}");