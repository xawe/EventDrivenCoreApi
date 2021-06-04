using Microsoft.EntityFrameworkCore;
using PostService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data
{
    public class PostData : IPostData
    {
        private readonly PostServiceContext _context;

        public PostData(PostServiceContext context)
        {
            _context = context;
        }
        public async Task<Post> AddPost(Post post)
        {
            post.PostId = 1;
            if (_context.Post.Any())
            {
                post.PostId = _context.Post.Max(x => x.PostId) + 1;
            }

            _context.Post.Add(post);
            await _context.SaveChangesAsync();
            return post;            
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _context.Post.Include(x => x.User).ToListAsync();           
        }
    }
}
