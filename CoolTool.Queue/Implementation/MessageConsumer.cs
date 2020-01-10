using CoolTool.QueueProvider.DataAccess;
using CoolTool.QueueProvider.Infrastructure;
using CoolTool.QueueProvider.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoolTool.QueueProvider.Implementation
{
    public class MessageConsumer : BaseRabbitMqClient, IMessageConsumer
    {
        private List<Queue> _Queues;

        private readonly ISettingsProvider _SettingsProvider;
        private readonly IMessageProcessor _MessageProcessor;
        private readonly ILogger<MessageConsumer> _Logger;

        public MessageConsumer(IOptions<RabbitMqClientOptions> options, ISettingsProvider settingsProvider, IMessageProcessor messageProcessor, ILogger<MessageConsumer> logger)
            : base(options, logger)
        {
            _SettingsProvider = settingsProvider
                                ?? throw new ArgumentException($"Argument {nameof(settingsProvider)} is null.", nameof(settingsProvider));
            _MessageProcessor = messageProcessor;
            _Logger = logger;

            _Queues = new List<Queue>();
            _Logger.LogInformation("MessageConsumer created");
        }

        public void StartConsume()
        {
            _Logger.LogInformation("Starting consume");
            var consumer = InitConsumer();


            foreach (var queue in _Queues)
            {
                try
                {
                    DeclareQueue(queue.Name);
                    Channel.BasicConsume(queue.Name, true, consumer);
                }
                catch (Exception e)
                {
                    _Logger.LogError(e, $"connection to the queue {queue.Name} failed.");
                    _Queues.Remove(queue);
                    StartConsume();
                    return;
                }
            }
            _Logger.LogInformation("Consume started");
        }

        public void StopConsume()
        {
            if (Channel.IsClosed) return;
            Channel.Close();
            _Logger.LogInformation("Consume stopped");
        }

        public Task RefreshConfigs()
        {
            var newSettings = _SettingsProvider.GetQueueToListen();
            if (newSettings == null)
            {
                var errorMessage = $"Argument {nameof(newSettings)} is null or empty";
                _Logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            _Queues = newSettings;
            _Logger.LogInformation("Consume stopped");
            return Task.CompletedTask;
        }

        private EventingBasicConsumer InitConsumer()
        {
            _Logger.LogInformation("InitConsumer started");

            if (Channel.IsOpen)
                Channel.Close();

            EnsureChanel();
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (chanel, ea) =>
            {
                Task.Run(async () => await _MessageProcessor.ProcessMessageAsync(ea));
            };

            _Logger.LogInformation("Consumer initiated");
            return consumer;
        }

    }
}
