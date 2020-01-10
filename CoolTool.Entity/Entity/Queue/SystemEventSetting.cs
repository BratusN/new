using CoolTool.DataAccess.Entity.System;

namespace CoolTool.DataAccess.Entity
{
    public class SystemEventSetting
    {
        public long SystemEventSettingsId { get; set; }

        public SystemEventType SystemEventType { get; set; }

        public Queue Queue { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
    }
}
