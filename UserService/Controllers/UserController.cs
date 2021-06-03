using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Entities.User>> PostUser(Entities.User user)
        {
            user.ID = 1;
            if (_context.User.Any())
            {
                user.ID = _context.User.Max(x => x.ID) + 1;
            }
            _context.User.Add(user);
            await _context.SaveChangesAsync();             
            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }
            
    }
}
