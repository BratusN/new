using CoolTool.UserService.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CoolTool.UserService.Api.Controllers
{
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _AccountService;

        public AccountController(IAccountService accountService)
        {
            _AccountService = accountService;
        }
    }
}
