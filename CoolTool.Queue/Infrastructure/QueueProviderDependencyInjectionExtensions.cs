using CoolTool.QueueProvider.Implementation;
using CoolTool.QueueProvider.Infrastructure;
using CoolTool.QueueProvider.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CoolTool.QueueProvider
{
    public static class QueueProviderDependencyInjectionExtensions
    {

        /// <summary>
        /// inject services for QueueProvider
        /// ISettingsProvider injected must be injected separately
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <example>
        /// var opt = new RabbitMqClientOptions();
        /// Configuration.Bind("RabbitMq", opt);</example>
        /// services.AddRabbitMqClient(new RabbitMqClientOptions());
        /// <returns></returns>
        public static IServiceCollection AddRabbitMqClient(
            this IServiceCollection services,
            RabbitMqClientOptions option)
        {
            try
            {
                if (option is null)
                {
                    throw new ArgumentException($"Can't get section {nameof(option)}");
                }

                services.Configure<RabbitMqClientOptions>(opt =>
                    {
                        opt.HostName = option.HostName;
                        opt.Port = option.Port;
                        opt.UserName = option.UserName;
                        opt.Password = option.Password;
                        opt.VirtualHost = option.VirtualHost;
                        opt.AutomaticRecoveryEnabled = option.AutomaticRecoveryEnabled;
                        opt.TopologyRecoveryEnabled = option.TopologyRecoveryEnabled;
                        opt.RequestedConnectionTimeout = option.RequestedConnectionTimeout;
                        opt.RequestedHeartbeat = option.RequestedHeartbeat;
                    });

                RegisterQueueServices(services);

                return services;
            }
            catch (Exception e)
            {
                throw new Exception("RabbitMqClient dependency injections failed", e);
            }
        }

        /// <summary>
        /// inject all services for QueueProvider
        /// ISettingsProvider injected must be injected separately
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">section that contain rabbitMq configuration</param>
        /// <example>
        /// services.AddRabbitMqClient(Configuration.GetSection("RabbitMq"));
        /// </example>>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMqClient(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                if (configuration is null)
                    throw new ArgumentException($"Can't get section {nameof(configuration)}");

                services.Configure<RabbitMqClientOptions>(configuration.Bind);
                RegisterQueueServices(services);

                return services;
            }
            catch (Exception e)
            {
                throw new Exception("RabbitMqClient dependency injections failed", e);
            }
        }

        private static void RegisterQueueServices(IServiceCollection services)
        {
            services.AddSingleton<IHandlerResolver, HandlerResolver>();
            services.AddSingleton<IMessageConsumer, MessageConsumer>();
            services.AddSingleton<IMessageProducer, SupervisionMessageProducer>();
            services.AddSingleton<IMessageProcessor, MessageProcessor>();
            services.AddSingleton<IBaseQueueClient, BaseRabbitMqClient>();

            //TODO remove
            services.AddSingleton<ISettingsProvider, TempSettingProvider>();
            services.Configure<RabbitHandlerOptions>(opt =>
            {
                opt.Assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };
            });
        }
    }
}
