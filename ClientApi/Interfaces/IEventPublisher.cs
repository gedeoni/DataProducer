using System.Threading.Tasks;

namespace ClientApi.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishEvent(object paymentEvent);
    }
}