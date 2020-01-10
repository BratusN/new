using Microsoft.Extensions.Logging;
using System;

namespace CoolTool.QueueProvider.Dto
{
    public class LogDto<TState>
    {
        public Guid Sender { get; set; }

        public LogLevel LogLevel { get; set; }

        public EventId EventId { get; set; }

        public TState State { get; set; }

        public Exception Exception { get; set; }

        public Func<TState, Exception, string> Formatter { get; set; }
    }
}
