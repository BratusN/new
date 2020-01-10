using CoolTool.QueueProvider.DataAccess;
using CoolTool.QueueProvider.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace CoolTool.QueueProvider.Implementation
{
    /// <summary>
    /// message producer with logging and resending if sending failed
    /// </summary>
    public class SupervisionMessageProducer : MessageProducer
    {
        /// <summary>
        /// sent messages awaiting confirmation from broker
        /// </summary>
        private readonly ConcurrentDictionary<ulong, string> _OutstandingConfirms;

        private readonly ILogger<SupervisionMessageProducer> _Logger;

        public SupervisionMessageProducer(IOptions<RabbitMqClientOptions> options, ISettingsProvider settingsProvider, ILogger<SupervisionMessageProducer> logger)
            : base(options, settingsProvider, logger)
        {
            _Logger = logger;
            _OutstandingConfirms = new ConcurrentDictionary<ulong, string>();
            PublisherConfirms();
            _Logger.LogInformation("SupervisionMessageProducer created");
        }

        /// <summary>
        /// registers send callback handlers
        /// </summary>
        protected void PublisherConfirms()
        {
            Channel.ConfirmSelect();
            Channel.BasicReturn += ChannelOnBasicReturn;
            Channel.BasicNacks += ChannelOnBasicNacks;
            Channel.BasicAcks += (sender, ea) =>
            {
                CleanOutstandingConfirms(ea.DeliveryTag);
            };
        }

        private void ChannelOnBasicNacks(object sender, BasicNackEventArgs ea)
        {
            if (_OutstandingConfirms.TryGetValue(ea.DeliveryTag, out var body))
            {
                _Logger.LogWarning("Consumer cant process message. Body: {0}", body);
                CleanOutstandingConfirms(ea.DeliveryTag);
            }
        }
        private void ChannelOnBasicReturn(object sender, BasicReturnEventArgs ea)
        {
            var body = Encoding.UTF8.GetString(ea.Body);
            _Logger.LogError("Sending message to the broker failed. Body: {0}", body);

            CleanOutstandingConfirms(body);

            if (IsReSent(ea))
            {
                Send(SystemEventType.DeadEnd, body);
                _Logger.LogError("Resend to dead letter. Body: {0}", body);
            }
            else
            {
                SendToQueue(ea.RoutingKey, body, true);
                _Logger.LogError("Resend to current queue. Body: {0}, queue {1}", body, ea.RoutingKey);
            }
        }

        private bool IsReSent(BasicReturnEventArgs ea)
        {
            return ea.BasicProperties.Headers != null &&
                   ea.BasicProperties.Headers.TryGetValue(ResendHeaderKey, out var resending) &&
                   (bool)resending;
        }

        protected override void SendToQueue(string queueName, string message, bool isResent = false)
        {
            _Logger.LogInformation("SendToQueue. queueName: {0}, message {1}, isResent {2}", queueName, message, isResent);
            _OutstandingConfirms.TryAdd(Channel.NextPublishSeqNo, message);
            base.SendToQueue(queueName, message, isResent);
        }

        private void CleanOutstandingConfirms(ulong sequenceNumber)
        {
            _OutstandingConfirms.TryRemove(sequenceNumber, out _);
        }
        private void CleanOutstandingConfirms(string body)
        {
            var (key, _) = _OutstandingConfirms.FirstOrDefault(x => string.Equals(x.Value, body));

            if (key.Equals(default))
            {
                _Logger.LogWarning("Failed to clean OutstandingConfirms because it do not contain such a message. Body: {0}", body);
                return;
            }

            CleanOutstandingConfirms(key);
        }
    }
}
