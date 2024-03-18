using SyntheticGenerator.Generators;
using SyntheticGenerator.Helpers;

namespace SyntheticGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.Configure<ReplayOrdersOptions>(builder.Configuration.GetSection("ReplayOrdersOptions"));
            builder.Configuration.AddCommandLine(args);
            builder.Services.AddHostedService<ReplayOrders>();

            var host = builder.Build();
            host.Run();
        }
    }
}