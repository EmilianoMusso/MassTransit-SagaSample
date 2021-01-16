using System;

namespace MassTransit.SagaSample.Models.DTO
{
    public class Product: BaseEntity
    {
        public string Code { get; set; }
        public int Quantity { get; set; }
    }
}
