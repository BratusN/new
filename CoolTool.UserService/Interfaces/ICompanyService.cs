using CoolTool.Entity.User;
using System.Threading.Tasks;

namespace CoolTool.UserService.Interfaces
{
    public interface ICompanyService
    {
        Task<Company> CreateCompanyAsync(string name);

        void DeleteUserCompany(UserInfo user);
    }
}
