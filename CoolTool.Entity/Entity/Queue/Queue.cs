using System.Collections.Generic;

namespace CoolTool.DataAccess.Entity
{
    public class Queue
    {
        public long QueueId { get; set; }
        public string Name { get; set; }

        public List<SystemEventSetting> Events { get; set; }
    }
}
