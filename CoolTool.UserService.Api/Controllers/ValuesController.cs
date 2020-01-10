using CoolTool.QueueProvider.DataAccess;
using CoolTool.QueueProvider.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CoolTool.UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private IMessageProducer _Producer;
        private IMessageConsumer _Consumer;

        public ValuesController(IMessageProducer handler, IMessageConsumer consumer)
        {
            _Producer = handler;
            _Consumer = consumer;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _Producer.Send(SystemEventType.None, new { Id = 1 });
            _Consumer.RefreshConfigs();
            _Consumer.StartConsume();
           // _Consumer.StopConsume();

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
