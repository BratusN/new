using CoolTool.QueueProvider.DataAccess;
using Newtonsoft.Json;

namespace CoolTool.Dto
{
    public class QueueMessage
    {
        public SystemEventType EventType { get; set; }

        public string Data { get; set; }

        public int ReceiveCount { get; set; }

        public string Error { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
