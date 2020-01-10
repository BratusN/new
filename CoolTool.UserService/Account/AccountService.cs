using AutoMapper;
using CoolTool.DataAccess.DbContexts;
using CoolTool.Dto;
using CoolTool.Entity.User;
using CoolTool.UserService.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using IdentityUser = CoolTool.Entity.Identity.IdentityUser;

namespace CoolTool.UserService.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly UserServiceDbContext _UserServiceDbContext;
        private readonly ICompanyService _CompanyService;
        private readonly IMapper _Mapper;

        public AccountService(UserManager<IdentityUser> userManager, ICompanyService companyService,
            UserServiceDbContext userServiceDbContext, IMapper mapper)
        {
            _UserManager = userManager;
            _CompanyService = companyService;
            _UserServiceDbContext = userServiceDbContext;
            _Mapper = mapper;
        }

        public async Task<UserInfo> CreateUserAsync(LocalRegisterDto userDto)
        {
            using (var transaction = _UserServiceDbContext.Database.BeginTransaction())
            {
                try
                {
                    var identityUser = await AddIdentityUser(userDto.Email, userDto.Password, userDto.Phone);

                    var user = await AddUserInfo(identityUser, userDto.FirstName, userDto.LastName);

                    var company = await _CompanyService.CreateCompanyAsync(userDto.Company);
                    user.Company = company;

                    _UserServiceDbContext.SaveChanges();
                    transaction.Commit();

                    return user;
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    return null;
                }

            }
        }

        public async Task<UserInfo> CreateUserAsync(ExternalRegisterDto userDto)
        {
            using (var transaction = _UserServiceDbContext.Database.BeginTransaction())
            {
                try
                {
                    var identityUser = await AddIdentityUser(userDto.Email);
                    await _UserManager.AddLoginAsync(identityUser, new UserLoginInfo(userDto.Provider,
                        userDto.ProviderUserId, userDto.Provider));
                    var user = await AddUserInfo(identityUser, userDto.FirstName, userDto.LastName);
                    await _UserServiceDbContext.SaveChangesAsync();

                    transaction.Commit();
                    return user;
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    return null;
                }
            }
        }



        public Task AddActions(UserInfo user)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInfoDto> GetUserInfoAsync(long userId)
        {
            var user = await _UserServiceDbContext.UserInfos.FindAsync(userId);

            return user == null ? null : _Mapper.Map<UserInfoDto>(user);
        }

        public async Task DeleteUserAsync(long userId)
        {
            var user = _UserServiceDbContext.UserInfos.Find(userId);

            if (user == null) return;

            using (var transaction = _UserServiceDbContext.Database.BeginTransaction())
            {
                try
                {
                    _CompanyService.DeleteUserCompany(user);

                    await _UserManager.DeleteAsync(user.IdentityUser);

                    _UserServiceDbContext.SaveChanges();

                    transaction.Commit();

                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        private async Task<IdentityUser> AddIdentityUser(string email, string password = null, string phone = null)
        {
            var identityUser = new IdentityUser
            {
                UserName = email,
                Email = email,
                PhoneNumber = phone
            };

            var result = string.IsNullOrWhiteSpace(password)
                ? await _UserManager.CreateAsync(identityUser)
                : await _UserManager.CreateAsync(identityUser, password);

            if (!result.Succeeded) throw new Exception("identity user creation failed");

            return identityUser;
        }

        private async Task<UserInfo> AddUserInfo(IdentityUser identityUser, string firstName, string lastName)
        {
            var userInfo = new UserInfo
            {
                FirstName = firstName,
                LastName = lastName,
                IdentityUser = identityUser
            };
            await _UserServiceDbContext.UserInfos.AddAsync(userInfo);

            return userInfo;
        }
    }
}
