using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data
{
    public interface IPostData
    {
        Task<Entities.Post> AddPost(Entities.Post post);

        Task<List<Entities.Post>> GetAllPosts();
    }
}
