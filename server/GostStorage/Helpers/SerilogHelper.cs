using Serilog;
using Serilog.Events;

namespace GostStorage.Helpers;

public static class SerilogHelper
{
    public static void AddLoggerServices(this IServiceCollection services)
    {
        services
            .AddSingleton(Log.Logger);
    }

    public static void GetConfiguration(this LoggerConfiguration loggerConfiguration)
    {
        const string logFormat =
            "[{Timestamp:yyyy.MM.dd HH:mm:ss:ms}] [{Level}] [T-{TraceId}] {Message}{NewLine}{Exception}";

        loggerConfiguration
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .MinimumLevel.Is(LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Async(option => { option.Console(LogEventLevel.Information, logFormat); });
    }
}