using SABlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SABlog.Repository
{
    public class CategoryRepository : BlogDatabaseEntities1, ICategoryRepository
    {

        private readonly BlogDatabaseEntities1 _context;

        public CategoryRepository(BlogDatabaseEntities1 context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.Where(it => it.Deleted == false).OrderBy(it => it.CreatedAt);
        }

       public int AddCategory(Category CategoryEntity)
        {
            if (CategoryEntity == null ) return -1;
            var category = _context.Categories.Where(it => it.Name == CategoryEntity.Name && it.Deleted == false).FirstOrDefault();
            if (category != null) return -1;

            _context.Categories.Add(CategoryEntity);
            _context.SaveChanges();
            return CategoryEntity.CategoryId;
        }

       public void DeleteCategory(int CategoryId)
        {
            Category c = _context.Categories.Find(CategoryId);
            c.Deleted = true;
            _context.Entry(c).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Category GetCategoryByName(string categoryName)
        {
            return _context.Categories.Where(it => it.Name == categoryName && it.Deleted == false).FirstOrDefault();
        }

        public string UpdateCategory(string emri, int id, string pershkrimi)
        {
            if (id == null) return "Ndodhi nje gabim";
            var catekz = _context.Categories.Where(it => it.Name == emri && it.Deleted == false).FirstOrDefault();
            if(catekz == null)
            {
                Category c = _context.Categories.Where(it =>it.CategoryId == id && it.Deleted == false).FirstOrDefault();
                c.Name = emri;
                c.Description = pershkrimi;
                _context.Entry(c).State = EntityState.Modified;

                _context.SaveChanges();

            }
            else
            {
                return "Ekziston nje kategori e tille";
            }

            return "Kategoria u ndryshua me sukses";
        }
    }
}