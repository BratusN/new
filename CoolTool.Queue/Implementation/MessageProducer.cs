using CoolTool.Dto;
using CoolTool.QueueProvider.DataAccess;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemEventSetting = CoolTool.QueueProvider.DataAccess.SystemEventSetting;

namespace CoolTool.QueueProvider.Interfaces
{
    public class MessageProducer : Implementation.BaseRabbitMqClient, IMessageProducer
    {
        private List<SystemEventSetting> _SystemEventSettings;
        private readonly ISettingsProvider _SettingsProvider;
        private readonly ILogger<MessageProducer> _Logger;

        public MessageProducer(IOptions<RabbitMqClientOptions> options, ISettingsProvider settingsProvider, ILogger<MessageProducer> logger)
            : base(options, logger)
        {
            _SettingsProvider = settingsProvider;
            _Logger = logger;

            _SystemEventSettings = new List<SystemEventSetting>();

            Task.Run(async () => await RefreshConfigs());
        }

        public void Send(QueueMessage message)
        {
            _Logger.LogInformation("Send. message: {0}", message);
            var queuesToSend = GetQueuesForEvent(message.EventType);

            if (!queuesToSend.Any())
            {
                _Logger.LogWarning($"There is no queue to send for the eventType {Enum.GetName(typeof(SystemEventType), message.EventType)}. Data: {message.Data}");
                return;
            }

            foreach (var queueSetting in queuesToSend)
            {
                SendToQueue(queueSetting.Queue.Name, JsonConvert.SerializeObject(message));
            }
        }

        public virtual void Send(SystemEventType eventType, object data, string error = null)
        {
            _Logger.LogInformation("Send. eventType: {0}, data: {1}", eventType, data);
            var message = new QueueMessage
            {
                Data = GetDataAsString(data),
                EventType = eventType,
                Error = error
            };
            Send(message);
        }

        private string GetDataAsString(object data)
        {
            switch (data)
            {
                case string strValue:
                    return strValue;
                case byte[] bytes:
                    return Encoding.UTF8.GetString(bytes);
                default:
                    return JsonConvert.SerializeObject(data);
            }
        }

        /// <summary>
        /// Replace old settings with queuesSettings
        /// </summary>
        public Task RefreshConfigs()
        {
            _Logger.LogInformation("RefreshConfigs");
            var newSettings = _SettingsProvider.GetEventSettings();
            if (newSettings is null)
            {
                var errorMessage = $"Argument {nameof(newSettings)} is null. Settings provider - {_SettingsProvider.GetType()}";
                _Logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            _SystemEventSettings = newSettings;

            return Task.CompletedTask;
        }

        protected virtual void SendToQueue(string queueName, string body, bool isResend = false)
        {
            _Logger.LogInformation($"SendToQueue. {nameof(queueName)}: {queueName}, {nameof(body)}:{body}, {nameof(isResend)}: {isResend}");
            DeclareQueue(queueName);
            IBasicProperties properties = null;
            if (isResend)
            {
                properties = Channel.CreateBasicProperties();

                properties.Headers = new Dictionary<string, object>
                {
                    {ResendHeaderKey, true }
                };
            }

            Channel.BasicPublish("", queueName, true, properties, Encoding.UTF8.GetBytes(body));
        }

        private List<SystemEventSetting> GetQueuesForEvent(SystemEventType eventType)
        {
            var queuesToSend =
                _SystemEventSettings
                    .Where(eventSettings => eventSettings.SystemEventType == eventType && eventSettings.IsActive).ToList();

            return queuesToSend;
        }
    }
}