using CoolTool.Dto;
using CoolTool.QueueProvider.DataAccess;
using CoolTool.QueueProvider.Infrastructure;
using CoolTool.QueueProvider.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CoolTool.QueueProvider.Implementation
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IMessageProducer _MessageProducer;
        private readonly IHandlerResolver _HandlerResolver;
        private int MaxReceiveCount = 5;
        private readonly ILogger<MessageProcessor> _Logger;

        public MessageProcessor(IHandlerResolver handlerResolver, IMessageProducer messageProducer, ILogger<MessageProcessor> logger)
        {
            _HandlerResolver = handlerResolver;
            _MessageProducer = messageProducer;
            _Logger = logger;
        }

        public async Task ProcessMessageAsync(BasicDeliverEventArgs ea)
        {
            _Logger.LogInformation("ProcessMessageAsync. body", ea.Body);
            var body = ea.Body;
            QueueMessage message = null;
            var messageStr = string.Empty;
            try
            {
                messageStr = Encoding.UTF8.GetString(body);
                message = JsonConvert.DeserializeObject<QueueMessage>(messageStr);
                var handler = _HandlerResolver.ResolveHandler(message);
                if (handler is null)
                {
                    var errorMessage = $"Handler for {Enum.GetName(typeof(SystemEventType), message.EventType)} is not defined";
                    _Logger.LogError(errorMessage, ea.Body);
                    throw new NotSupportedException(errorMessage);
                }

                await handler.HandleMessage(message);
            }
            catch (Exception e)
            {
                if (message != null && ++message.ReceiveCount < MaxReceiveCount)
                {
                    _MessageProducer.Send(message);
                    _Logger.LogError(e, "ProcessMessageAsync. Message processing failed. Body: {0}", messageStr);
                }
                else
                {
                    _MessageProducer.Send(SystemEventType.DeadEnd, body, JsonConvert.SerializeObject(e));
                    _Logger.LogError(e, "ProcessMessageAsync. Message not processed at all. body: {0}", messageStr);
                }
            }
        }
    }
}