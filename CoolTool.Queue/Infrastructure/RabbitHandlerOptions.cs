using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace CoolTool.QueueProvider
{
    public class RabbitHandlerOptions
    {
        public List<Assembly> Assemblies { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
