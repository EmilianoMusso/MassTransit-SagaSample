using MassTransit.SagaSample.Models.DTO;
using System;
using System.Collections.Generic;

namespace MassTransit.SagaSample.Models.Events
{
    public interface IWarehouseChecked
    {
        Guid CorrelationId { get; }
        List<Product> Products { get; }
    }
}
