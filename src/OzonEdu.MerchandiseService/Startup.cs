using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.GrpcServices;
using OzonEdu.MerchandiseService.Services.Interfaces;
using ServiceStack.Text;

namespace OzonEdu.MerchandiseService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<ConsumerHostedService>();
            services.AddSingleton<IMerchandiseService, Services.MerchandiseService>();
            services.AddSingleton<IProducer<int, Order>>(
                provider =>
                {
                    var config = new ProducerConfig
                    {
                        BootstrapServers = "localhost:9092",
                    };
                    var builder = new ProducerBuilder<int, Order>(config);
                    builder.SetValueSerializer(new JsonSerializer<Order>());
                    
                    return builder.Build();
                });
            
            services.AddSingleton<IConsumer<int, Order>>(
                provider =>
                {
                    var config = new ConsumerConfig()
                    {
                        BootstrapServers = "localhost:9092",
                        GroupId = "1",
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        EnableAutoCommit = false,
                    };
                    var builder = new ConsumerBuilder<int, Order>(config);
                    builder.SetValueDeserializer(new JsonSerializer<Order>());
                    
                    return builder.Build();
                });
            
            services.AddInfrastructureServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MerchandiseServiceGrpcService>();
                endpoints.MapControllers();
            });
        }
    }
}