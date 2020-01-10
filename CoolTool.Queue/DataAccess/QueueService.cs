using System;

namespace CoolTool.QueueProvider.DataAccess
{
    public class QueueService
    {
        public long QueueServiceId { get; set; }
        public Queue Queue { get; set; }
        public Guid ServiceGuid { get; set; }
    }
}
