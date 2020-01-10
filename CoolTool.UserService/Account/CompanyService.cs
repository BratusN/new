using CoolTool.Entity.User;
using CoolTool.UserService.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using CoolTool.DataAccess.DbContexts;

namespace CoolTool.UserService.Account
{
    public class CompanyService : ICompanyService
    {
        private readonly UserServiceDbContext _UserServiceContext;

        public CompanyService(UserServiceDbContext userServiceContext)
        {
            _UserServiceContext = userServiceContext;
        }

        public async Task<Company> CreateCompanyAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var company = new Company
            {
                Name = name
            };

            await _UserServiceContext.Companies.AddAsync(company);
            _UserServiceContext.SaveChanges();

            return company;
        }

        public void DeleteUserCompany(UserInfo user)
        {
            if (user?.Company == null)
                throw new ArgumentNullException();
            if (!_UserServiceContext.UserInfos.Any(x => x.Company == user.Company && x.UserInfoId != user.UserInfoId))
                _UserServiceContext.Companies.Remove(user.Company);
            _UserServiceContext.SaveChanges();
        }
    }
}
