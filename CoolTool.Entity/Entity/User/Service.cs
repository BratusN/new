using System;
using System.Collections.Generic;

namespace CoolTool.Entity.User
{
    public class Service
    {
        public long ServiceId { get; set; }
        public string Name { get; set; }
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }

        public List<Action> Action { get; set; }
    }
}
