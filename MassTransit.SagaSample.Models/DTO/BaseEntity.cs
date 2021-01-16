using Newtonsoft.Json;

namespace MassTransit.SagaSample.Models.DTO
{
    public class BaseEntity
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
