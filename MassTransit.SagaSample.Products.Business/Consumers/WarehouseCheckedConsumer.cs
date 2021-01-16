using MassTransit.SagaSample.Models.Events;
using System;
using System.Threading.Tasks;

namespace MassTransit.SagaSample.Products.Business.Consumers
{
    public class WarehouseCheckedConsumer : IConsumer<IWarehouseChecked>
    {
        public async Task Consume(ConsumeContext<IWarehouseChecked> context)
        {
            await Console.Out.WriteLineAsync($"Current Warehouse check:");

            foreach(var p in context.Message.Products)
            {
                await Console.Out.WriteLineAsync($"{p.Code}\t\t{p.Quantity}");
            }
        }
    }
}
