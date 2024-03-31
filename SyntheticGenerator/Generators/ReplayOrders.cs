using System.Text.Json;
using Microsoft.Extensions.Options;
using SyntheticGenerator.Helpers;

namespace SyntheticGenerator.Generators
{
    public class ReplayOrders : BackgroundService
    {
        private readonly ILogger<ReplayOrders> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly Random random = new();
        private readonly ReplayOrdersOptions _options;

        public ReplayOrders(ILogger<ReplayOrders> logger, IOptions<ReplayOrdersOptions> options, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Your primary work here
            _logger.LogInformation("Starting the primary work...");
            await DoPrimaryWork();

            // Stop the application after the work is done
            _logger.LogInformation("Stopping the application");
            _hostApplicationLifetime.StopApplication();
        }

        private async Task DoPrimaryWork()
        {
            _logger.LogInformation("retreiving configuration...");
            _options.ValidateObject();

            var windowStartTimeStr = _options.WindowStartTimeStr ?? DateTime.Parse("2024-03-15T12:15:00");
            var windowEndTimeStr = _options.WindowEndTimeStr ?? DateTime.Parse("2024-03-15T12:15:00");
            var lambda = _options.Lambda ?? 120;
            var numberOfEvents = _options.NumberOfEvents ?? 10;

            var windowStartTime = windowStartTimeStr;
            var windowEndTime = windowEndTimeStr;
            var windowDuration = windowEndTime - windowStartTime;

            using (var writer = GetOutputStream())
            {
                for (var i = 0; i < numberOfEvents; i++)
                {
                    var eventDurationInSeconds = GeneratePoisson(lambda);
                    var eventDuration = TimeSpan.FromSeconds(eventDurationInSeconds);

                    // Distribute events evenly across the time window
                    var fraction = (double)i / numberOfEvents;
                    var offset = TimeSpan.FromTicks((long)(fraction * windowDuration.Ticks)).RoundedToSeconds();
                    var eventStartTime = windowStartTime.Add(offset);
                    var eventEndTime = eventStartTime.Add(eventDuration);

                    var evt = new Event
                    {
                        StartTime = eventStartTime,
                        EndTime = eventEndTime,
                        TotalTime = eventDurationInSeconds
                    };

                    var json = JsonSerializer.Serialize(evt);
                    await writer.WriteLineAsync(json);
                }
            }

            _logger.LogInformation("Primary work is done");
        }

        // Poisson distribution generator
        private int GeneratePoisson(double lambda)
        {
            var l = Math.Exp(-lambda);
            var k = 0;
            double p = 1;

            do
            {
                k++;
                var u = random.NextDouble();
                p *= u;
            } while (p > l);

            return k - 1;
        }
        private StreamWriter GetOutputStream()
        {
            var outFile = _options.OutputFile;
            // Check if the outFile parameter has a value
            if (!string.IsNullOrEmpty(outFile))
            {
                // Return a StreamWriter for the specified file
                return new StreamWriter(outFile);
            }
            else
            {
                // Return a StreamWriter for the standard output stream
                return new StreamWriter(Console.OpenStandardOutput())
                {
                    AutoFlush = true
                };
            }
        }
    }


    internal class Event
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TotalTime { get; set; }
    }



}
