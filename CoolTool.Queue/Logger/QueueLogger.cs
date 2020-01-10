using CoolTool.Dto;
using CoolTool.QueueProvider.DataAccess;
using CoolTool.QueueProvider.Dto;
using CoolTool.QueueProvider.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace CoolTool.QueueProvider.Logger
{
    public class QueueLogger : ILogger
    {
        private readonly Guid _Sender;
        private readonly IMessageProducer _Producer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Guid of running assembly</param>
        /// <param name="producer">MessageProducer to send message to LogService</param>
        public QueueLogger(Guid sender, IMessageProducer producer)
        {
            _Sender = sender;
            _Producer = producer;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var log = new LogDto<TState>
            {
                Sender = _Sender,
                LogLevel = logLevel,
                EventId = eventId,
                State = state,
                Exception = exception,
                Formatter = formatter
            };
            var message = new QueueMessage
            {
                EventType = SystemEventType.Log,
                Data = JsonConvert.SerializeObject(log)
            };

            _Producer.Send(message);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
