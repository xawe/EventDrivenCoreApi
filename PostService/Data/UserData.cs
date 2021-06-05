using PostService.Entities;
using System.Linq;

namespace PostService.Data
{


    public class UserData : IUserData
    {
        
        public PostServiceContext dbContext { get; set; }

        public User AddUser(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return user;
        }

        public User UpdateUser(User user)
        {
            dbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();
            return user;
        }
    }
}
