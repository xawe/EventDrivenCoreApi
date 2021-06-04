using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using UserService.Data;


namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService.Data.UserServiceContext _context;

        public UserController(UserServiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserService.Entities.User>>> GetUser()
        {
            
            return await _context.User.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserService.Entities.User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var integrationEventData = JsonConvert.SerializeObject(new { id = user.ID, newname = user.Name });
            PublishToMessageQueue("user.update", integrationEventData);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Entities.User>> PostUser(Entities.User user)
        {
            try
            {
                
                user.ID = 1;
                if (_context.User.Any())
                {
                    user.ID = _context.User.Max(x => x.ID) + 1;
                }
                Console.WriteLine("Criando usuário ID " + user.ID);
                _context.User.Add(user);
                await _context.SaveChangesAsync();

                var integrationEventData = JsonConvert.SerializeObject(new { id = user.ID, name = user.Name });
                PublishToMessageQueue("user.add", integrationEventData);

                return CreatedAtAction("GetUser", new { id = user.ID }, user);
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
