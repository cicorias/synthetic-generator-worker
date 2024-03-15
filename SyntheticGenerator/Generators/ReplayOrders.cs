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
            double lambda = 1.0; // Lambda for Poisson distribution, in seconds
            int numberOfEvents = 10; // Total number of events you want to generate

            string filePath = "events.jsonl"; // Path to the JSON Lines file
            using var fileStream = new StreamWriter(filePath);

            var random = new Random(42);
            DateTime previousEventTime = DateTime.Now;

            for (int i = 0; i < numberOfEvents; i++)
            {
                double timeDifference = -Math.Log(1.0 - random.NextDouble()) / lambda;
                DateTime eventTime;

                if (i == 0)
                {
                    // First event starts at the current time
                    eventTime = previousEventTime;
                }
                else
                {
                    // Subsequent events use the time difference
                    eventTime = previousEventTime.AddSeconds(timeDifference);
                }

                var eventObj = new Event
                {
                    StartTime = eventTime
                };

                var json = JsonSerializer.Serialize(eventObj);
                await fileStream.WriteLineAsync(json);

                //previousEventTime = previousEventTime.AddSeconds(-Math.Log(1 - random.NextDouble()) * lambda);
                //var order = new
                //{
                //    OrderId = Guid.NewGuid(),
                //    EventTime = previousEventTime,
                //    EventType = "OrderPlaced"
                //};
                //var json = JsonSerializer.Serialize(order);
                //await fileStream.WriteLineAsync(json);
            }

            await Task.Delay(1000);
            _logger.LogInformation("Primary work is done");
        }

    }

    class Event
    {
        public DateTime StartTime { get; set; }
        //public DateTimeOffset DateOffset { get; set; }
    }
}
