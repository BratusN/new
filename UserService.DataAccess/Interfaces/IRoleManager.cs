using CoolTool.Entity.Identity;
using System;
using System.Threading.Tasks;

namespace UserService.DataAccess.Interfaces
{
    public interface IRoleManager : IBaseManager<Role>
    {
        Task AssignActionAsync(Action action);
        Task RemoveActionAsync(Action action);

        Task GetUsersAsync();
    }
}
