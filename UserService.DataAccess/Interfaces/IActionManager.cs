using System.Threading.Tasks;
using Action = CoolTool.Entity.User.Action;

namespace UserService.DataAccess.Interfaces
{
    public interface IActionManager : IBaseManager<Action>
    {
        Task ChangeActivationStatusAsync(bool newStatus);
    }
}
