using CoolTool.Dto;
using CoolTool.QueueProvider.DataAccess;
using System.Threading.Tasks;

namespace CoolTool.QueueProvider.Interfaces
{
    public interface IMessageProducer
    {
        /// <summary>
        /// sending a message to all queues that are subscribed to the eventType
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="data"></param>
        void Send(SystemEventType eventType, object data, string error = null);

        /// <summary>
        /// sending a message to all queues that are subscribed to the message.EventType
        /// </summary>
        /// <param name="message"></param>
        void Send(QueueMessage message);

        Task RefreshConfigs();
    }
}
