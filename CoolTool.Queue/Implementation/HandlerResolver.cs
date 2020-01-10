using CoolTool.Dto;
using CoolTool.QueueProvider.DataAccess;
using CoolTool.QueueProvider.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoolTool.QueueProvider.Implementation
{
    /// <summary>
    /// The list of handlers is obtained from all assemblies.
    /// Selects IMessageHandler implementations that have at least one HandlerAttribute.
    /// One event can have only one handler.
    /// </summary>
    public class HandlerResolver : IHandlerResolver
    {
        private Dictionary<SystemEventType, Type> _HandlerTypes;
        private readonly IServiceProvider _ServiceProvider;
        private readonly ILogger<HandlerResolver> _Logger;

        public HandlerResolver(IServiceProvider serviceProvider, IOptions<RabbitHandlerOptions> handlersAssemblies, ILogger<HandlerResolver> logger)
        {
            _ServiceProvider = serviceProvider;
            _Logger = logger;
            InitHandlers(handlersAssemblies.Value.Assemblies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueMessage"></param>
        /// <returns></returns>
        public IMessageHandler ResolveHandler(QueueMessage queueMessage)
        {
            _Logger.LogInformation("ResolveHandler. args: {0}", queueMessage);

            var eventType = queueMessage.EventType;
            var handlerType = _HandlerTypes.ContainsKey(eventType) ? _HandlerTypes[eventType] : null;

            if (handlerType == null)
            {
                var errorMessage = $"ResolveHandler. Handler not found. EventType: {Enum.GetName(typeof(SystemEventType), eventType)}";
                _Logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            return CreateHandler(handlerType);
        }

        private IMessageHandler CreateHandler(Type handlerType)
        {
            return (IMessageHandler)_ServiceProvider.GetService(handlerType);
        }

        private void InitHandlers(List<Assembly> assemblies)
        {
            _HandlerTypes = new Dictionary<SystemEventType, Type>();
            var handlerList = new List<Type>();

            foreach (var assembly in assemblies.Where(assembly => assembly != null))
            {
                handlerList.AddRange(assembly.GetTypes().Where(x =>
                    typeof(IMessageHandler).IsAssignableFrom(x) &&
                    x.CustomAttributes.Any(attr => attr.AttributeType == typeof(HandlerAttribute)) &&
                    x.IsClass &&
                    !x.IsAbstract));
            }

            foreach (var handler in handlerList)
            {
                foreach (var attribute in handler.GetCustomAttributes<HandlerAttribute>())
                {
                    if (_HandlerTypes.ContainsKey(attribute.EventType))
                    {
                        _Logger.LogWarning($"InitHandlers. More than one event handler registered for {Enum.GetName(typeof(SystemEventType), attribute.EventType)}");
                        continue;
                    }

                    _HandlerTypes.Add(attribute.EventType, handler);
                }
            }
            _Logger.LogInformation("InitHandlers. Handlers initiated");
        }
    }
}
