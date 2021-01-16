using MassTransit.SagaSample.Models.DTO;
using MassTransit.SagaSample.Models.Events;
using MassTransit.SagaSample.Products.Business.Consumers;
using MassTransit.Util;
using System;
using System.Timers;

namespace MassTransit.SagaSample.Products.Business
{
    class Program
    {
        static IBus _bus;
        static bool _isRunning;

        static string[] productCodes = new string[] { "TEST001", "TEST002", "TEST003", "TEST004", "TEST005" };
        static void Main()
        {
            _bus = CreateRabbitBus();
            _isRunning = true;

            var t = new Timer()
            {
                Interval = 5000,
                Enabled = true
            };

            t.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                var random = new Random(Environment.TickCount);

                var product = new Product()
                {
                    Code = productCodes[random.Next(productCodes.Length-1)],
                    Quantity = random.Next(0, 1000)
                };

                _bus.Publish<IProductGenerated>(new { CorrelationId = NewId.NextGuid(), Product = product });
                Console.WriteLine($"Product generated: {product}");

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

                config.ReceiveEndpoint("masstransit-sample-" + nameof(WarehouseCheckedConsumer), c =>
                {
                    c.Consumer(() => new WarehouseCheckedConsumer());
                });
            });

            TaskUtil.Await(() => bus.StartAsync());
            return bus;
        }
    }
}
