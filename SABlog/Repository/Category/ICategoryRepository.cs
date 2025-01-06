using SABlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SABlog.Repository
{
    public interface ICategoryRepository : IDisposable
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryByName(string category);
        
        int AddCategory(Category CategoryEntity);
        string UpdateCategory(string emri, int id, string pershkrimi);
        void DeleteCategory(int CategoryId);
    }
}
