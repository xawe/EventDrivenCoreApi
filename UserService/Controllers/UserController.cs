using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserService.Data;
using Service.Common.Messages;


namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService.Data.UserServiceContext _context;
        private readonly UserService.Data.IUserData _userData;
        public UserController(UserServiceContext context, UserService.Data.IUserData userData)
        {
            _context = context;
            _userData = userData;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserService.Entities.User>>> GetUser()
        {
            return await _userData.GetAllUsers();                        
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserService.Entities.User user)
        {            
            var r = await _userData.UpdateUser(user);
            var integrationEventData = JsonConvert.SerializeObject(new { id = r.ID, newname = r.Name });
            PublishToMessageQueue(QueueName.UserUpdate, integrationEventData);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Entities.User>> PostUser(Entities.User user)
        {
            try
            {
                var r = await _userData.AddUser(user);
                var integrationEventData = JsonConvert.SerializeObject(new { id = r.ID, name = r.Name });
                PublishToMessageQueue(QueueName.UserAdd, integrationEventData);;

                return CreatedAtAction("GetUser", new { id = r.ID }, r);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex -- " + ex.Message + " >>>>> " + ex.InnerException.Message) ;
                throw;
            }
            
        }


        private void PublishToMessageQueue(string integrationEvent, string eventData)
        {
            var factory = new ConnectionFactory();            
            using (var connection  = factory.CreateConnection())
            {                
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(eventData);
                    channel.BasicPublish(exchange: "user",
                        routingKey: integrationEvent,
                        mandatory: true,
                        basicProperties: null,
                        body: body);
                }                
            }                        
        }
            
    }
}
