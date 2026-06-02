using System.Threading.Tasks;

namespace RoutineCore.Application.Services.Interfaces
{
    public interface IEventsPublisher
    {
        Task PublishAsync<T>(T message) where T : class;
    }
}
