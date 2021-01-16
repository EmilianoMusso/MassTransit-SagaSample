using MassTransit.SagaSample.Warehouse.Business.Consumers;
using MassTransit.SagaSample.Warehouse.Business.Persistor;
using MassTransit.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace MassTransit.SagaSample.Warehouse.Business
{
    class Program
    {
        static IBus _bus;
        static MemoryDbContext _context;

        static void Main()
        {
            var options = new DbContextOptionsBuilder<MemoryDbContext>()
                            .UseInMemoryDatabase(databaseName: "TEST")
                            .Options;

            _context = new MemoryDbContext(options);

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
                    c.Consumer(() => new ProductGeneratedConsumer(_context));
                });
            });

            TaskUtil.Await(() => bus.StartAsync());
            return bus;
        }
    }
}
