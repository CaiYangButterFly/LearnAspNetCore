using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using SkiaSharp.QrCode;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    // [Authorize]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    /// <summary>
    /// 生成二维码图片
    /// </summary>
    [HttpGet(Name = "GenerateQRCode")]
    public ActionResult GenerateQRCode()
    {
        var content = "testtesttest";
        using var generator = new QRCodeGenerator();

// Generate QrCode
        var qr = generator.CreateQrCode(content, ECCLevel.L);

// Render to canvas
        var info = new SKImageInfo(512, 512);
        using var surface = SKSurface.Create(info);
        var canvas = surface.Canvas;
        canvas.Render(qr, info.Width, info.Height);
        // Output to Stream -> File
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        
        using var stream = System.IO.File.OpenWrite(@"hoge.png");
        data.SaveTo(stream);

        return Ok();
    }
}