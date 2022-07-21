using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using ServiceStack.Text;

namespace OzonEdu.MerchandiseService
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly IConsumer<int, Order> _consumer;
        private readonly Logger<ConsumerHostedService> _logger;

        public ConsumerHostedService(IConsumer<int, Order> consumer, ILogger<ConsumerHostedService> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("Order");
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = _consumer.Consume(cancellationToken);
                _logger.LogInformation($"Message ID: {message.Message.Key}, Value {message.Message.Value}");
            }
            
            _consumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}