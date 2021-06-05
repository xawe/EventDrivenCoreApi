using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Data
{
    public interface IUserData
    {
        public Task<List<UserService.Entities.User>> GetAllUsers();

        public Task<Entities.User> AddUser(Entities.User user);

        public Task<Entities.User> UpdateUser(Entities.User user);
    }
}
