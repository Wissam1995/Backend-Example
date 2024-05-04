using Microsoft.AspNetCore;
using Serilog;
using WeatherCheckApi;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseSerilog((context, config) => { config.ReadFrom.Configuration(context.Configuration); });
    }
}
