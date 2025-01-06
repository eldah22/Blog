using PagedList;
using SABlog.Models;
using SABlog.Repository;
using SABlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SABlog.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : BaseController
    {
        private IPostRepository _postRepository;
        private ICategoryRepository _categoryRepository;

        public AdminController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public AdminController()
        {
            _postRepository = new PostRepository(new BlogDatabaseEntities1());
        }
        

        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPostList()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            var draw = Request.Form["draw"];
            var sortColumn =  Request.Form["order[0][column]"];
            string sortColumnDirection = Request.Form["order[0][dir]"];
            string searchValue = Request.Form["search[value]"];
            int pageSize = Convert.ToInt32(Request.Form["length"] ?? "0");
            int pageNumber = Convert.ToInt32(Request.Form["start"] ?? "0");
            var data = _postRepository.GetAll().Where(it =>
                   //it.PostStatu.Status == "Draft" &&
                   !it.Deleted
                );

          
            //get total count of data in table
            totalRecord = data.Count();
            // search data when search value found
            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(it => it.Title.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            // get total count of records after search
            filterRecord = data.Count();
            //sort data
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                switch (sortColumnDirection)
                {
                    case "asc":
                        switch (sortColumn)
                        {
                            case "1":
                                data = data.OrderBy(it => it.Title);
                                break;
                            case "2":
                                data = data.OrderBy(it => it.PostFiles.Count);
                                break;
                            case "3":
                                data = data.OrderBy(it => it.User.UserName);
                                break;
                            case "4":
                                data = data.OrderBy(it => it.CreatedAt);
                                break;
                            default:
                                data = data.OrderBy(it => it.PostId);
                                break;
                        }
                        break;
                    default:
                        switch (sortColumn)
                        {
                            case "1":
                                data = data.OrderByDescending(it => it.Title);
                                break;
                            case "2":
                                data = data.OrderByDescending(it => it.PostFiles.Count);
                                break;
                            case "3":
                                data = data.OrderByDescending(it => it.User.UserName);
                                break;
                            case "4":
                                data = data.OrderByDescending(it => it.CreatedAt);
                                break;
                            default:
                                data = data.OrderByDescending(it => it.PostId);
                                break;
                        }
                        break;
                }
            }
            //pagination
            var postList = data.Skip(pageNumber).Take(pageSize).ToList();
            Debug.WriteLine(postList.Count() + " " + totalRecord + " "+ filterRecord);
            List<PendingPost> posts = new List<PendingPost>(postList.Count);
            postList.ForEach(it =>
                posts.Add(
                    new PendingPost(
                            it.PostId,
                            it.Title,
                            it.PostFiles.Count,
                            it.User.UserName,
                            it.CreatedAt.ToString("d/M/yyyy - HH:mm", CultureInfo.InvariantCulture),
                            it.PostStatu.Status
                        )
                )
            ); ; ;


            var returnObj = new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = posts
        };

            return Json(returnObj);
        }


        public ActionResult ApprovePost(int id)
        {
            try
            {
                Post p = _postRepository.GetPostById(id);
                if (p == null) throw new Exception("Postimi nuk u gjet");
                p.PostStatusId = _postRepository.GetApprovedPostStatusId();
                p.PublicatedAt = DateTime.Now;
                _postRepository.UpdatePost(p);
            }catch(Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult UnapprovePost(int id)
        {
            try
            {
                Post p = _postRepository.GetPostById(id);
                if (p == null) throw new Exception("Postimi nuk u gjet");
                p.PostStatusId = _postRepository.GetUnApprovedPostStatusId();
                _postRepository.UpdatePost(p);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return RedirectToAction("Index", "Admin");
        }


        public ActionResult DeletePost(int id)
        {
            try
            {
                Post p = _postRepository.GetPostById(id);
                if (p == null) throw new Exception("Postimi nuk u gjet");
                _postRepository.DeletePost(p);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult DeleteEldaTest(int CategoryId)
        {
            try
            {
                //if (!ModelState.IsValid) throw new Exception("Informacion i pa plote!");
               // _categoryRepository.DeleteCategory(CategoryId);
            }
            catch (Exception e)
            {
                return Json(new { response = "deshtim" });
            }

            return Json(new { response = "success" });
        }

    }
}