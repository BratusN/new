using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace CoolTool.QueueProvider.Infrastructure
{
    public interface IMessageProcessor
    {
        Task ProcessMessageAsync(BasicDeliverEventArgs ea);
    }
}
