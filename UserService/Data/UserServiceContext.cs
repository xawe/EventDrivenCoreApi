using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UserService.Data
{
    public class UserServiceContext: DbContext
    {
        public UserServiceContext(DbContextOptions<UserServiceContext> options) : base(options)
        {
            
        }

        public DbSet<Entities.User> User { get; set; }
    }
}
