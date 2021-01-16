using MassTransit.SagaSample.Models.DTO;
using MassTransit.SagaSample.Models.Events;
using MassTransit.SagaSample.Warehouse.Business.Persistor;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.SagaSample.Warehouse.Business.Consumers
{
    class ProductGeneratedConsumer : IConsumer<IProductGenerated>
    {
        private readonly MemoryDbContext dbContext;

        public ProductGeneratedConsumer(MemoryDbContext _context)
        {
            this.dbContext = _context;
        }

        public async Task Consume(ConsumeContext<IProductGenerated> context)
        {
            await Console.Out.WriteLineAsync($"Product generated - CorrelationId: {context.Message.CorrelationId}");

            if (context.Message.Product.Quantity > 0)
            {
                await context.Publish<IProductStock>(new
                {
                    CorrelationId = context.Message.CorrelationId,
                    Product = context.Message.Product
                });

                dbContext.ProductsFlow.Add(new ProductsFlow()
                {
                    Code = context.Message.Product.Code,
                    Quantity = context.Message.Product.Quantity
                });

                await dbContext.SaveChangesAsync();

                var warehouseCheck = dbContext.ProductsFlow.GroupBy(x => x.Code).Select(x => new Product() { Code = x.Key, Quantity = x.Sum(y => y.Quantity) });

                await context.Publish<IWarehouseChecked>(new
                {
                    CorrelationId = context.Message.CorrelationId,
                    Products = warehouseCheck
                });
            }
        }
    }
}
