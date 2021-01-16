using MassTransit.SagaSample.Models.Events;
using MassTransit.SagaSample.Warehouse.Business.Consumers;
using MassTransit.Util;

namespace MassTransit.SagaSample.Warehouse.Business
{
    class Program
    {
        static IBus _bus;
        static void Main(string[] args)
        {
            _bus = CreateRabbitBus();

            while (true) { }
        }

        private static IBus CreateRabbitBus()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host("rabbitmq://localhost");

                config.ReceiveEndpoint("masstransit-sample-" + nameof(ProductGeneratedConsumer), c =>
                {
                    c.Consumer(() => new ProductGeneratedConsumer());
                });
            });

            TaskUtil.Await(() => bus.StartAsync());
            return bus;
        }
    }
}
