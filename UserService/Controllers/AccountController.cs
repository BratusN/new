//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using CoolTool.UserService.Dto;
//using CoolTool.UserService.Interfaces;
//using Microsoft.AspNetCore.Mvc;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace CoolTool.UserServiceApi.Controllers
//{
//    [Route("api/Account")]
//    public class AccountController : Controller
//    {
//        private readonly IAccountService _AccountService;

//        public AccountController(IAccountService accountService)
//        {
//            _AccountService = accountService;
//        }

//        [HttpGet("Test")]
//        public async Task<string> Test()
//        {
//           var user1 = await _AccountService.CreateUserAsync(new LocalRegisterDto
//            {
//                Company = "company",
//                Email = "test" + DateTime.Now.Ticks + "@mail.com",
//                FirstName = "testUser1",
//                Password = "password"
//            });

//           await _AccountService.DeleteUserAsync(user1.UserId);


//           var user2 = await _AccountService.CreateUserAsync(new ExternalRegisterDto
//           {
//               Email = "test" + DateTime.Now.Ticks + "@mail.com",
//               FirstName = "testUser2",
//               Provider = "google",
//               ProviderUserId = "some_id"
//           });

//           await _AccountService.DeleteUserAsync(user2.UserId);


//            return "ok";
//        }
//        // GET: api/<controller>
//        [HttpGet]
//        public IEnumerable<string> Get()
//        {
//            return new string[] { "value1", "value2" };
//        }

//        // GET api/<controller>/5
//        [HttpGet("{id}")]
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST api/<controller>
//        [HttpPost]
//        public void Post([FromBody]string value)
//        {
//        }

//        // PUT api/<controller>/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody]string value)
//        {
//        }

//        // DELETE api/<controller>/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
//    }
//}
