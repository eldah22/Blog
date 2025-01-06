using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SABlog.Repository;
using SABlog.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using SABlog.ViewModels;

namespace SABlog.Controllers
{
    //[Authorize]
    public class CategoryController : BaseController
    {
        private ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public CategoryController()
        {
            _categoryRepository = new CategoryRepository(new BlogDatabaseEntities1());
        }

        // GET: Post
        public ActionResult Index()
        {
            List<Category> cats = _categoryRepository.GetAllCategories().ToList();
            return View(cats);
        }

        public ActionResult Details(string categoryName)
        {
            var view = "~/Views/Category/Details.cshtml";

            var parent = _categoryRepository.GetCategoryByName(categoryName);
            if (parent == null) return RedirectToAction("Index", "Home");
       

            return View(view, parent);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (TempData["Failed"] != null)
            {
                ViewBag.Failed = TempData["Failed"];
            }
            List<Category> allCategories = _categoryRepository.GetAllCategories().ToList();
            CategoryCreate c = new CategoryCreate(allCategories);

            return View(c);
        }


        [HttpPost]
        public ActionResult Create(CategoryCreate category)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Informacion i pa plote!");
                Category c = _categoryRepository.GetCategoryByName(category.Name);
                if (c != null) throw new Exception("Kategoria ekziston");
                Category newCategory = new Category();
                newCategory.CategoryId = 0;
                newCategory.Name = category.Name;
                newCategory.Description = category.Description;
                newCategory.CreatedAt = DateTime.Now;
                newCategory.Deleted = false;

                
                _categoryRepository.AddCategory(newCategory);
                return RedirectToAction("Index", "Category");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                TempData["Failed"] = e.Message;
                Console.WriteLine(e.Message);
                return Create();
            }

            return Create();

        }

      



    }
}