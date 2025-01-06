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
    [Authorize]
    public class HomeController : BaseController
    {
        private IPostRepository _postRepository;
        private ICategoryRepository _categoryRepository;

        public HomeController(IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }

        public HomeController()
        {
            _postRepository = new PostRepository(new BlogDatabaseEntities1());
            _categoryRepository = new CategoryRepository(new BlogDatabaseEntities1());
        }

        public ActionResult Index(string orderBy, int? page)
        {
            ViewBag.OrderBy = String.IsNullOrEmpty(orderBy) ? "date" : orderBy;
            List<Post> posts = _postRepository.GetAll().Where(it => (!it.Deleted & it.PostStatu.Status == "Publik") == true).ToList();
            if (orderBy == null) orderBy = "date";
            switch (orderBy)
            {
                case "date":
                    posts = posts.OrderByDescending(it => it.CreatedAt).ToList();
                    break;
                
                default:
                    posts = posts.OrderByDescending(it => it.CreatedAt).ToList();
                    break;
            }
            int pageNumber = (page ?? 1);
            int pageSize = (posts.Count > 20) ?  20: posts.Count;
            if (pageSize == 0) pageSize = 1;
            return View(posts.ToPagedList(pageNumber, pageSize));
        }
        

        [HttpGet]
        public ActionResult Search(string query, string dateFrom, string dateTo,  int? page)
        {
            ViewBag.dateFrom = dateFrom;
            ViewBag.dateTo = dateTo;

            if (query == null) query = "";
            if (dateFrom == null || dateFrom == "")
            {
                dateFrom = DateTime.MinValue.ToString("d/M/yyyy hh:mm:ss tt");
            }

            if (dateTo == null || dateTo == "")
            {
                dateTo = DateTime.Now.ToString("d/M/yyyy hh:mm:ss tt");
            }

           
            int pageNumber = (page ?? 1);

            if (TempData["Failed"] != "")
            {
                ViewBag.Failed = TempData["Failed"];
            }


            ViewBag.query = query;

            IEnumerable<Post> posts = _postRepository.GetAll().Where(it =>
                !it.Deleted &&
                it.PostStatu.Status == "Publik" &&
                it.CreatedAt >= DateTime.ParseExact(dateFrom, "d/M/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture) &&
                it.CreatedAt <= DateTime.ParseExact(dateTo, "d/M/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)
                );

         
            posts = posts.Where(it =>
                    it.Title.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    it.Content.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    it.User.UserName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    it.User.FirstName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    it.User.LastName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
            );
            List<Post> filteredPosts = posts.ToList();
            int pageSize = (filteredPosts.Count > 20) ? 20 : filteredPosts.Count;
            if (pageSize == 0) pageSize = 1;

            return View(filteredPosts.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Search(string query, string dateFrom, string dateTo, List<CheckBox> categories)
        {

            if (categories == null) categories = new List<CheckBox>();
            string cats = "";
            if(categories.Count > 0)
            {
                cats = String.Join(",", categories.Where(it => it.Selected).Select(it => it.Name));
            }

            DateTime dateF, dateT;
            try
            {
                dateFrom = DateTime.ParseExact(dateFrom, "d/M/yyyy", CultureInfo.InvariantCulture).ToString("d/M/yyyy hh:mm:ss tt");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                dateFrom = null;
            }
            try
            {
                dateTo = DateTime.ParseExact(dateTo, "d/M/yyyy", CultureInfo.InvariantCulture).ToString("d/M/yyyy hh:mm:ss tt").Replace("12:00:00 AM", "11:59:59 PM");
            }catch(Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                dateTo = null;
            }


            return RedirectToAction("Search", "Home", new { query = query, dateFrom = dateFrom, dateTo = dateTo, categories = cats });
        }


        public ActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}