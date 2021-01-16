using MassTransit.SagaSample.Models.DTO;
using System;

namespace MassTransit.SagaSample.Models
{
    public interface IMessage
    {
        Guid CorrelationId { get; }
        Product Product { get; }
    }
}
