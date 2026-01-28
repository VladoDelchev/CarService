using CarService.BL;
using CarService.DL;
using CarService.DL.Interfaces;
using CarService.DL.Repositories;
using Mapster;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace CarService.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services
                .AddDataLayer(builder.Configuration)
                .AddBusinessLogicLayer();

            builder.Services.AddMapster();

            builder.Services.AddControllers();
         
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Car Service", Version = "v1" });
            });

            builder.Services.AddHealthChecks();
            builder.Services.AddSingleton<ICarRepository, CarRepository>();
            var app = builder.Build();

            app.MapHealthChecks("/health");

            app.UseAuthorization();

            app.MapControllers();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "Car Service 2 V1");
            });

            app.UseSwagger();

            app.Run();
        }
    }
}
