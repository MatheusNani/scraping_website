using ConsoleApp1.Interfaces;
using ConsoleApp1.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;


public class Exercise
{

    // DI, Serilog, Settings  

    static void Main()
    {
        var builder = new ConfigurationBuilder();
        BuildConfig(builder);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger.Information("Application Starting...");

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddTransient<IExecuteService, ExecuteService>();
                services.AddTransient<ICalculateService, CalculateService>();
                services.AddTransient<IApiGetContentRequestService, ApiGetContentRequestService>();
            })
            .UseSerilog()
            .Build();

        var serv = ActivatorUtilities.CreateInstance<ExecuteService>(host.Services);

        serv.Run();
    }

    static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables();
    }

}
