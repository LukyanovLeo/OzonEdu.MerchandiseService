using System;
using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;

namespace OzonEdu.MerchandiseService
{
    public class OrderProducer : IOrderProducer
    {
        private readonly IProducer<long, Order> _producer;

        public OrderProducer(IProducer<long, Order> producer)
        {
            _producer = producer;
        }
        
        public void Publish(Order order)
        {
            var message = new Message<long, Order>
            {
                Key = order.Id,
                Value = order,
            };
            
            _producer.Produce("Order", message);
        }
    }

    public class JsonSerializer<TMessage> : ISerializer<TMessage>, IDeserializer<TMessage>
    {
        public byte[] Serialize(TMessage data, SerializationContext context)
        {
            return data switch
            {
                string => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data)),
                _ => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data)),
            };
        }

        public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return isNull ? default : JsonSerializer.Deserialize<TMessage>(data);
        }
    }
}