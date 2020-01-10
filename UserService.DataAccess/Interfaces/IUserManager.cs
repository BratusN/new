using CoolTool.Entity.Identity;
using CoolTool.Entity.User;
using System.Threading.Tasks;

namespace UserService.DataAccess.Interfaces
{
    public interface IUserManager : IBaseManager<User>
    {
        Task AddActionAsync(Action action);
        Task RemoveActionAsync(Action action);

        Task SetRole(Role role);
    }
}
