using SABlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SABlog.Repository
{
    public class PostRepository : BlogDatabaseEntities1, IPostRepository
    {

        private readonly BlogDatabaseEntities1 _context;

        public PostRepository(BlogDatabaseEntities1 context)
        {
            _context = context;
        }


        public IEnumerable<Post> GetAll()
        {
            return _context.Posts.OrderBy(it => it.CreatedAt);
        }

        public int GetApprovedPostStatusId()
        {
            return _context.PostStatus.Where(it => it.Status == "Publik").FirstOrDefault().PostStatusId;
        }

        public int GetUnApprovedPostStatusId()
        {
            return _context.PostStatus.Where(it => it.Status == "Refuzuar").FirstOrDefault().PostStatusId;
        }

        public int AddPost(Post postEntity, List<Category> selectedCategories, List<PostFile> files)
        {
            if (postEntity == null) return -1;
            User userOfPost = _context.Users.Find(postEntity.UserId);
            if(userOfPost.Role.Name == "admin")
            {
                postEntity.PostStatusId = _context.PostStatus.Where(it => it.Status == "Publik").FirstOrDefault().PostStatusId;
            }
            else
            {
                postEntity.PostStatusId = _context.PostStatus.Where(it => it.Status == "Draft").FirstOrDefault().PostStatusId;
            }
            
            Post post = _context.Posts.Add(postEntity);
            foreach (var c in selectedCategories)
            {
                PostCategory p = new PostCategory();
                p.PostId = post.PostId;
                p.CategoryId = c.CategoryId;
                p.CreatedAt = DateTime.Now;
                _context.PostCategories.Add(p);
            }
            foreach (var f in files)
            {
                PostFile tmp = new PostFile();
                tmp.Content = f.Content;
                tmp.FileName = f.FileName;
                tmp.Extension = f.Extension;
                tmp.FileType = f.FileType;
                tmp.PostId = post.PostId;
                tmp.CreatedAt = DateTime.Now;
                _context.PostFiles.Add(tmp);
            }
            _context.SaveChanges();
            return postEntity.PostId;
        }

        public void DeletePost(Post post)
        {
            post.Deleted = true;
            _context.Entry(post).State = EntityState.Modified;
            _context.SaveChanges();
        }

       public IEnumerable<Post> GetAllPostsFromUser(int userId)
        {
            return _context.Posts.Where(it => it.UserId == userId).OrderBy(it => it.CreatedAt);
        }

        public Post GetPostById(int postId)
        {
            var post = _context.Posts.Where(it => it.PostId == postId).FirstOrDefault();
            return post;
        }

       public int UpdatePost(Post postEntity)
        {
            if (postEntity == null) return -1;

            _context.Entry(postEntity).State = EntityState.Modified;
            _context.SaveChanges();
            return postEntity.UserId;
        }
        public string UpdatePost1(string postTitle, string postContent, int postId)
        {
            if (postContent == null || postTitle == null || postContent == "" || postTitle == "" || postId == null) return "Posti nuk mund te editohet";

            var post = GetPostById(postId);
            if (post != null)
            {
                if (post.Title != postTitle)
                {
                    post.Title = postTitle;

                }
                if (post.Content != postContent)
                {
                    post.Content = postContent;
                }
                post.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                return "Success";
            }
            else
            {
                return "Posti nuk mund te editohet";
            }

        }
    }
}