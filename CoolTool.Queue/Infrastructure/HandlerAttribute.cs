using System;
using CoolTool.QueueProvider.DataAccess;

namespace CoolTool.QueueProvider
{
    public class HandlerAttribute : Attribute
    {
        public SystemEventType EventType { get; set; }
    }
}
