namespace CoolTool.QueueProvider.Interfaces
{
    public interface IBaseQueueClient
    {
        void DeclareQueue(string name);

        void DeleteQueue(string name);

        void OpenConnection();
    }
}
