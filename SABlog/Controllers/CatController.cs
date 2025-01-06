using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azure;
using SABlog.Models;
using SABlog.Repository;

namespace SABlog.Controllers
{
    public class CatController : Controller
    {
        // GET: Cat
        private ICategoryRepository _categoryRepository;

        public CatController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public CatController()
        {
            _categoryRepository = new CategoryRepository(new BlogDatabaseEntities1());
        }

        public ActionResult Update(string emri, int id, string pershkrimi)
        {
            string response = "";
            try
            {
                if (!ModelState.IsValid) throw new Exception("Informacion i pa plote!");
                response = _categoryRepository.UpdateCategory( emri,  id,  pershkrimi);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                TempData["Failed"] = "Deshtim";
                Console.WriteLine(e.Message);
            }

            return Json(new { response });
        }


        public ActionResult DeleteElda(int CategoryId)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Informacion i pa plote!");
                _categoryRepository.DeleteCategory(CategoryId);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                TempData["Failed"] = "Deshtim";
                Console.WriteLine(e.Message);
            }

            return Json(new { response = "success" });
        }
    }
}