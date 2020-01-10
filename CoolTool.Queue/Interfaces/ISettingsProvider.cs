using System.Collections.Generic;
using CoolTool.QueueProvider.DataAccess;

namespace CoolTool.QueueProvider.Interfaces
{
    public interface ISettingsProvider
    {
        List<SystemEventSetting> GetEventSettings();


        List<Queue> GetQueueToListen();
    }
}
