using CoolTool.QueueProvider.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Diagnostics;

namespace CoolTool.QueueProvider.Implementation
{
    public class BaseRabbitMqClient : IBaseQueueClient
    {
        protected const string ResendHeaderKey = "resending";

        protected IConnection Connection;
        protected IModel Channel;
        private readonly ConnectionFactory _Factory;

        private readonly ILogger<BaseRabbitMqClient> _Logger;

        public BaseRabbitMqClient(IOptions<RabbitMqClientOptions> options, ILogger<BaseRabbitMqClient> logger)
        {
            if (options is null)
            {
                throw new ArgumentException($"Argument {nameof(options)} is null.", nameof(options));
            }

            _Logger = logger;


            var optionsValue = options.Value;
            _Factory = new ConnectionFactory
            {
                HostName = optionsValue.HostName,
                Port = optionsValue.Port,
                UserName = optionsValue.UserName,
                Password = optionsValue.Password,
                VirtualHost = optionsValue.VirtualHost,
                AutomaticRecoveryEnabled = optionsValue.AutomaticRecoveryEnabled,
                TopologyRecoveryEnabled = optionsValue.TopologyRecoveryEnabled,
                RequestedConnectionTimeout = optionsValue.RequestedConnectionTimeout,
                RequestedHeartbeat = optionsValue.RequestedHeartbeat
            };

            EnsureChanel();
            _Logger.LogInformation("BaseRabbitMqClient created. {0}:{1}", optionsValue.HostName, optionsValue.Port);
        }

        public void DeclareQueue(string name)
        {
            try
            {
                EnsureChanel();
                Channel.QueueDeclare(name, true, false, false);
                _Logger.LogInformation($"DeclareQueue. Name: {name}");
            }
            catch (Exception e)
            {
                _Logger.LogError(e, $"DeclareQueue failed. Name: {name}");

                throw;
            }
        }
        public void DeleteQueue(string name)
        {
            try
            {
                EnsureChanel();
                Channel.QueueDelete(name);
                _Logger.LogInformation($"Queue deleted. Name: {name}");
            }
            catch (Exception e)
            {
                _Logger.LogError(e, $"Queue deletion failed. Name: {name}");

                throw;
            }
        }

        public void OpenConnection()
        {
            _Logger.LogInformation($"OpenConnection. Host: {Connection.Endpoint}");

            if (Connection.IsOpen)
                Connection.Close();
            Channel = null;

            Connection = _Factory.CreateConnection();
            EnsureChanel();
        }

        protected void EnsureChanel()
        {
            if (Channel != null && Channel.IsOpen) return;
            if (Connection == null || !Connection.IsOpen)
                Connection = _Factory.CreateConnection();

            Channel = Connection.CreateModel();

            var concurrentMessagesNumber = Process.GetCurrentProcess().Threads.Count - 1;
            Channel.BasicQos(0, (ushort)concurrentMessagesNumber, false);
            _Logger.LogInformation($"EnsureChanel. Chanel was created. Host: {Connection.Endpoint}");
        }
    }
}
