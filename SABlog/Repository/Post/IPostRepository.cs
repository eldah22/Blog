using SABlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SABlog.Repository
{
    public interface IPostRepository : IDisposable
    {
        IEnumerable<Post> GetAllPostsFromUser(int userId);
        IEnumerable<Post> GetAll();
        Post GetPostById(int postId);
        int AddPost(Post postEntity, List<Category> selectedCategories, List<PostFile> files);
        int UpdatePost(Post postEntity);
        void DeletePost(Post post);

        int GetApprovedPostStatusId();
        int GetUnApprovedPostStatusId();
        string UpdatePost1(string postTitle, string postContent, int postId);
    }
        
}
