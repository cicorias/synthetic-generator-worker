using SyntheticGenerator.Generators;
using SyntheticGenerator.Helpers;

namespace SyntheticGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            // NOTE: this is here to ensure stderr config value for "Logging:Console:LogToStandardErrorThreshold": "Trace",
            builder.Logging.AddConsole(options => {
                options.LogToStandardErrorThreshold = LogLevel.Trace;
                options.FormatterName = "simple";
            }).AddSimpleConsole(options => {
                options.IncludeScopes = true;
                options.SingleLine = true;
                options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
            });

            builder.Services.Configure<ReplayOrdersOptions>(builder.Configuration.GetSection("ReplayOrdersOptions"));
            builder.Configuration.AddCommandLine(args);
            builder.Services.AddHostedService<ReplayOrders>();

            var host = builder.Build();
            host.Run();
        }
    }
}