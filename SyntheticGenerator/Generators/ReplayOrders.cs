using System;
using System.Collections.Generic;
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
            // Implementation of your primary work
            _logger.LogInformation("...");

            int numberOfEvents = int.Parse("50");
            DateTime windowStartTime = DateTime.Parse("2024-03-15T12:00:00");
            DateTime windowEndTime = DateTime.Parse("2024-03-15T12:15:00");
            string filePath = "events.txt";


            GenerateEventTimes(numberOfEvents, windowStartTime, windowEndTime, filePath);

        await Task.Delay(1000);
            _logger.LogInformation("Primary work is done");
        }

        static void GenerateEventTimes(int numberOfEvents, DateTime windowStartTime, DateTime windowEndTime, string filePath)
        {
            var random = new Random();

            using (var writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < numberOfEvents; i++)
                {
                    DateTime startTime = GenerateRandomDateTime(windowStartTime, windowEndTime, random);
                    double durationInHours = SampleExponential(random, 1.0); // Exponential sample for event duration
                    DateTime endTime = startTime.AddHours(durationInHours);

                    // Ensure endTime is within the window
                    if (endTime > windowEndTime)
                    {
                        endTime = windowEndTime;
                    }

                    writer.WriteLine($"Start Time: {startTime}, End Time: {endTime}");
                }
            }
        }

        static DateTime GenerateRandomDateTime(DateTime start, DateTime end, Random random)
        {
            TimeSpan range = end - start;
            TimeSpan randomSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));
            return start + randomSpan;
        }

        static double SampleExponential(Random random, double mean)
        {
            // Using the inverse transform sampling method for the exponential distribution
            return -mean * Math.Log(1 - random.NextDouble());
        }

    }

    class Event
    {
        public DateTime StartTime { get; set; }
        //public DateTimeOffset DateOffset { get; set; }
    }
}
