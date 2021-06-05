using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data
{
    public interface IUserData
    {
        Data.PostServiceContext dbContext { get; set; }

        Entities.User AddUser(Entities.User user);
        Entities.User UpdateUser(Entities.User user);

    }
}
