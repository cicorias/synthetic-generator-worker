using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SyntheticGenerator.Generators
{
    public class ReplayOrders : BackgroundService
    {
        private readonly ILogger<ReplayOrders> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly Random random = new();

        public ReplayOrders(ILogger<ReplayOrders> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
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
            _logger.LogInformation("...");
            var windowStartTimeStr = "2024-03-15T12:00:00";
            var windowEndTimeStr = "2024-03-15T12:15:00";
            var outputFile = "events.jsonl";
            var lambda = double.Parse("120");
            var numberOfEvents = int.Parse("500");

            var windowStartTime = DateTime.Parse(windowStartTimeStr);
            var windowEndTime = DateTime.Parse(windowEndTimeStr);
            var windowDuration = windowEndTime - windowStartTime;

            using (var writer = new StreamWriter(outputFile))
            {
                writer.WriteLine("StartTime,EndTime,TotalTime");

                for (var i = 0; i < numberOfEvents; i++)
                {
                    var eventDurationInSeconds = GeneratePoisson(lambda);
                    var eventDuration = TimeSpan.FromSeconds(eventDurationInSeconds);

                    // Distribute events evenly across the time window
                    var fraction = (double)i / numberOfEvents;
                    var offset = TimeSpan.FromTicks((long)(fraction * windowDuration.Ticks)).RoundedToSeconds();

                    //double totalSeconds = offset.TotalSeconds;
                    //int roundedSeconds = (int)Math.Round(totalSeconds);
                    //TimeSpan roundedTimeSpan = TimeSpan.FromSeconds(roundedSeconds);

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

            Console.WriteLine("File created successfully.");

            await Task.Delay(1000);
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
    }

    internal class Event
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TotalTime { get; set; }
    }

    internal static class Extensions
    {

        public static TimeSpan RoundedToSeconds(this TimeSpan ts)
        {
            return TimeSpan.FromSeconds((int)Math.Round(ts.TotalSeconds));
        }
        
    }
}
