using MassTransit.SagaSample.Models.DTO;
using MassTransit.SagaSample.Models.Events;
using MassTransit.Util;
using System;
using System.Timers;

namespace MassTransit.SagaSample.Products.Business
{
    class Program
    {
        static IBus _bus;
        static bool _isRunning;

        static void Main(string[] args)
        {
            _bus = CreateRabbitBus();
            _isRunning = true;

            var t = new Timer()
            {
                Interval = 10_000,
                Enabled = true
            };

            t.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                var random = new Random(Environment.TickCount);

                var product = new Product()
                {
                    Code = string.Concat("PRD", Environment.TickCount),
                    Quantity = random.Next(1, 1000)
                };

                _bus.Publish<IProductGenerated>(new { CorrelationId = NewId.NextGuid(), Product = product });
                Console.WriteLine($"Product generated: {product.ToString()}");

                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Q) _isRunning = false;
            };

            t.Start();

            while (_isRunning) { }
        }

        private static IBus CreateRabbitBus()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host("rabbitmq://localhost");
            });

            TaskUtil.Await(() => bus.StartAsync());
            return bus;
        }
    }
}
