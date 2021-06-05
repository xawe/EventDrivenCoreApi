using Microsoft.AspNetCore.Mvc;
using PostService.Data;
using PostService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace PostService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {        
        private readonly IPostData _postData;

        public PostController( IPostData postData)
        {            
            _postData = postData;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostService.Entities.Post>>> GetPost()
        {
            return await _postData.GetAllPosts();
        }

        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            var p = await _postData.AddPost(post);            
            return CreatedAtAction("GetPost", new { id = p.PostId }, p);
        }
        
    }
}
