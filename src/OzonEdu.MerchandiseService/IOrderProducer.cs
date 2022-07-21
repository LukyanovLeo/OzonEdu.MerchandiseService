using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;

namespace OzonEdu.MerchandiseService
{
    public interface IOrderProducer
    {
        void Publish(Order order);
    }
}