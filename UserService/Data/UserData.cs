using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public class UserData : IUserData
    {

        private readonly UserServiceContext _context;

        public UserData(UserServiceContext context)
        {
            _context = context;
        }        

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> AddUser(User user)
        {
            user.ID = 1;
            if (_context.User.Any())
            {
                user.ID = _context.User.Max(x => x.ID) + 1;
            }            
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;            
        }

        public async Task<User> UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
