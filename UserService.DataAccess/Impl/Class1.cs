using System;
using System.Threading.Tasks;
using UserService.DataAccess.Interfaces;
using Action = CoolTool.Entity.User.Action;

namespace UserService.DataAccess.Impl
{
    public class ActionManager : IActionManager
    {
        public Task<Action> CreateAsync(Action entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Action entity)
        {
            throw new NotImplementedException();
        }

        public Task<Action> Update(Action entity)
        {
            throw new NotImplementedException();
        }

        public Task ChangeActivationStatusAsync(bool newStatus)
        {
            throw new NotImplementedException();
        }
    }
}
