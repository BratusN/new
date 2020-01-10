using CoolTool.QueueProvider.Interfaces;
using System.Collections.Generic;
using CoolTool.QueueProvider.DataAccess;
using Queue = CoolTool.QueueProvider.DataAccess.Queue;
using SystemEventSetting = CoolTool.QueueProvider.DataAccess.SystemEventSetting;

namespace CoolTool.QueueProvider.Implementation
{
    public class TempSettingProvider : ISettingsProvider
    {
        public List<SystemEventSetting> GetEventSettings()
        {
            return new List<SystemEventSetting>{new SystemEventSetting
            {
                SystemEventType = SystemEventType.None,
                Queue = new Queue
                {
                    Name = "test",
                    QueueId = 1
                },
                IsActive = true
            }};
        }

        public List<Queue> GetQueueToListen()
        {
            return new List<Queue>
            {
                new Queue
                {
                    Name = "test",
                    QueueId = 1
                },
                new Queue
                {
                Name = "hello",
                QueueId = 1
            }
            };
        }

    }
}
