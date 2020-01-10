using CoolTool.Dto;

namespace CoolTool.QueueProvider.Interfaces
{
    public interface IHandlerResolver
    {
        IMessageHandler ResolveHandler(QueueMessage queueMessage);
    }

   
}
