using System.Threading.Tasks;

namespace CoolTool.QueueProvider.Interfaces
{
    public interface IMessageConsumer
    {
        void StartConsume();
        void StopConsume();

        Task RefreshConfigs();
    }
}
