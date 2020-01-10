using System;

namespace CoolTool.Entity.User
{
    public class Action
    {
        public long ActionId { get; set; }
        public string Name { get; set; }
        public long ServiceId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTs { get; set; }
        public DateTime UpdateTs { get; set; }

        public Service Service { get; set; }
    }
}
