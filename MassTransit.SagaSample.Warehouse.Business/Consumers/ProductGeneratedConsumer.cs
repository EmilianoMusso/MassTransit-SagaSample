using MassTransit.SagaSample.Models.Events;
using System;
using System.Threading.Tasks;

namespace MassTransit.SagaSample.Warehouse.Business.Consumers
{
    class ProductGeneratedConsumer : IConsumer<IProductGenerated>
    {
        public async Task Consume(ConsumeContext<IProductGenerated> context)
        {
            await Console.Out.WriteLineAsync($"Product generated - CorrelationId: {context.Message.CorrelationId}");
        }
    }
}
