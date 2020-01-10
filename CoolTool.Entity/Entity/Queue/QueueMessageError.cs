using CoolTool.DataAccess.Entity.System;

namespace CoolTool.DataAccess.Entity
{
    public class QueueMessageError
    {
        public long QueueMessageErrorId { get; set; }

        public SystemEventType SystemEvent { get; set; }

        public string Error { get; set; }
    }
}
