using MassTransit.SagaSample.Models.DTO;
using System;

namespace MassTransit.SagaSample.Models
{
    public interface IProductMessage
    {
        Guid CorrelationId { get; }
        Product Product { get; }
    }
}
