using Newtonsoft.Json;

namespace CoolTool.QueueProvider
{
    public class RabbitMqClientOptions
    {
        public string HostName { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 5672;

        public string UserName { get; set; } = "guest";

        public string Password { get; set; } = "guest";

        public string VirtualHost { get; set; } = "/";

        public bool AutomaticRecoveryEnabled { get; set; } = true;

        public bool TopologyRecoveryEnabled { get; set; } = true;

        public int RequestedConnectionTimeout { get; set; } = 60000;

        public ushort RequestedHeartbeat { get; set; } = 60;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
