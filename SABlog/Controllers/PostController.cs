using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SABlog.Repository;
using SABlog.Models;
using System.Diagnostics;
using System.IO;
using SABlog.ViewModels;
using System.Text.RegularExpressions;
using System.Globalization;
using static SABlog.Helpers.Helper;

namespace SABlog.Controllers
{
    [Authorize]
    public class PostController : BaseController
    {
        private IPostRepository _postRepository;
        private ICategoryRepository _categoryRepository;
        private Helpers.Helper helper = new Helpers.Helper();


        public PostController (IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }

        public PostController()
        {
            _postRepository = new PostRepository(new BlogDatabaseEntities1());
            _categoryRepository = new CategoryRepository(new BlogDatabaseEntities1());
        }

        // GET: Post
        public ActionResult Index(string username, int postId)
        {
            var post = _postRepository.GetPostById(postId);
            if (post == null || post.PostStatu.Status != "Publik" || post.Deleted) return RedirectToAction("Index", "Home");
            var view = "~/Views/Post/Details.cshtml";
            PostInfoViewModel postview = new PostInfoViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                PostStatusId = post.PostStatusId,
                PostStatus = post.PostStatu?.Status,
                UserId = post.UserId,
                UserName = post.User?.UserName,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                PublicatedAt = post.PublicatedAt,
                Deleted = post.Deleted,
                PostCategories = post.PostCategories.Select(pc => new CategoryViewModel
                {
                    CategoryId = pc.CategoryId,
                    CategoryName = pc.Category?.Name
                }).ToList(),
                PostFiles = post.PostFiles.Select(pf => new PostFileViewModel
                {
                    FileId = pf.FileId,
                    Content = pf.Content,
                    FileType = pf.FileType,
                    FileName = pf.FileName,
                    Extension = pf.Extension,
                    CreatedAt = pf.CreatedAt,
                    Deleted = pf.Deleted
                }).ToList(),
                User = new UserViewModel
                {
                    UserId = post.User.UserId,
                    UserName = post.User.UserName,
                    FirstName = post.User.FirstName,
                    LastName = post.User.LastName,
                    Email = post.User.Email,
                    EmailVerified = post.User.EmailVerified,
                    CreatedAt = post.User.CreatedAt
                }
            };
            return View(view, postview);
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
            List<Category> categories = _categoryRepository.GetAllCategories().ToList();
            List<CheckBox> SelectedCategories = new List<CheckBox>();
            List<HttpPostedFileBase> files = new List<HttpPostedFileBase>();
            foreach(var c in categories)
            {
                SelectedCategories.Add(new CheckBox(c.Name));
            }

            if (TempData["Failed"] != null)
            {
                ViewBag.Failed = TempData["Failed"];
            }

            return View("~/Views/Post/Create.cshtml", new PostCreate(categories, SelectedCategories, files));
        }

        [HttpPost]
        public ActionResult CreatePost(PostCreate post)
        {
            try
            {
                if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Auth"); }
                if (!ModelState.IsValid) throw new Exception("Informacion i pa plote!");
                if (post.PostTitle == null || post.PostTitle == "") throw new Exception("Ju lutem plotësoni fushën Titulli!");
                if (post.PostContent == null || post.PostContent == "") throw new Exception("Ju lutem plotësoni fushën Përshkrimi!");
                if (post.UploadedFiles == null) throw new Exception("Ju lutem ngarkoni të paktën një skedar!");

                List<PostFile> files = new List<PostFile>();
                if (post.UploadedFiles.Count > 5) throw new Exception("Nuk duhet të postosh më shumë se 5 Skedarë!");
                foreach (var p in post.UploadedFiles)
                {
                    if (p.ContentLength == 0) throw new Exception("Një nga skedarët ka hapesirën 0!");
                    else if (p.ContentLength > 5000000) throw new Exception("Një nga skedarët është më i madh se 5 MB!");

                    FileType fileType = helper.GetFileType(p.FileName);
                    if (fileType == FileType.Unsupported) throw new Exception($"{p.FileName} - Skedar i pa suportuar!");

                    byte[] bytes;
                    using (var stream = new MemoryStream())
                    {
                        p.InputStream.CopyTo(stream);
                        bytes = stream.ToArray();
                    }
                    string base64 = Convert.ToBase64String(bytes);

                    PostFile f = new PostFile();
                    f.Content = base64;
                    f.FileType = fileType.ToString();
                    f.FileName = Path.GetFileNameWithoutExtension(p.FileName);
                    f.Extension = Path.GetExtension(p.FileName);
                    f.CreatedAt = DateTime.Now;
                    files.Add(f);
                }


                List<Category> categories = _categoryRepository.GetAllCategories().ToList();
                List<Category> selectedCategories = new List<Category>();
                foreach (var c in post.SelectedCategories.Where(it => it.Selected).ToList())
                {
                    selectedCategories.Add(categories.Where(it => it.Name == c.Name).FirstOrDefault());
                }

                Post newPost = new Post();
                newPost.PostId = 0;
                newPost.Title = (post.PostTitle.Length <= 200) ? post.PostTitle : post.PostTitle.Substring(0, 200);
                newPost.Content = post.PostContent;
                newPost.UserId = post.UserId;
                newPost.CreatedAt = DateTime.Now;
                
                _postRepository.AddPost(newPost, selectedCategories, files);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                TempData["Failed"] = e.Message;
                Console.WriteLine(e.StackTrace);
                return CreatePost();
            }

            return RedirectToAction("Index", "Home");
        }


        public ActionResult DeletePost(int postId)
        {
            var obj = new { response = "success" };
            try
            {
                Post post = _postRepository.GetPostById(postId);
                if (post == null) throw new Exception("Posti nuk u gjet!");
                _postRepository.DeletePost(post);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + "\n" + e.StackTrace);
                obj = new { response = "failure" };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult UpdatePost(string postTitle, string postContent, int postId)
        {
            
            if (postContent == null || postTitle == null || postContent == "" || postTitle == "" || postId == null) return  Json(new { response = "Missed parameter" }); ;

          
            try
            {
                var resp = _postRepository.UpdatePost1(postTitle, postContent, postId);
                return Json(new { response = "success" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { response = "failure" });
            }

        }


    }
}