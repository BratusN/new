using System.Threading.Tasks;
using CoolTool.Dto;
using CoolTool.Entity.User;

namespace CoolTool.UserService.Interfaces
{
    public interface IAccountService
    {
        Task<UserInfo> CreateUserAsync(LocalRegisterDto user);
        Task<UserInfo> CreateUserAsync(ExternalRegisterDto user);
        Task AddActions(UserInfo user);

        Task<UserInfoDto> GetUserInfoAsync(long userId);

        Task DeleteUserAsync(long userId);
    }
}
