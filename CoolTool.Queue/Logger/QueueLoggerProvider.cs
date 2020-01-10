using CoolTool.QueueProvider.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace CoolTool.QueueProvider.Logger
{
    public class QueueLoggerProvider : ILoggerProvider
    {
        private readonly IMessageProducer _Producer;
        public QueueLoggerProvider(IMessageProducer producer)
        {
            _Producer = producer;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new QueueLogger(GetAssemblyGuid(), _Producer);
        }

        private Guid GetAssemblyGuid()
        {
            return Assembly.GetExecutingAssembly().GetType().GUID;
        }

        public void Dispose() { }
    }
}
