using System.Threading.Tasks;
using CoolTool.Dto;

namespace CoolTool.QueueProvider.Interfaces
{
    public interface IMessageHandler
    {
        Task HandleMessage(QueueMessage message);
    }
}
