namespace CoolTool.QueueProvider.DataAccess
{
    public class QueueMessageError
    {
        public long QueueMessageErrorId { get; set; }

        public SystemEventType SystemEvent { get; set; }

        public string Error { get; set; }
    }
}
