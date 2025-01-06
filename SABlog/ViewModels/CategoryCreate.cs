using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SABlog.Models;

namespace SABlog.ViewModels
{
    public class CategoryCreate
    {
        public string Name{ get; set; }
        public string Description{ get; set; }
        public string ParentCategory{ get; set; }
        public List<Category> Categories { get; set; }


        public CategoryCreate(List<Category> cats)
        {
            Categories = cats;
        }

        public CategoryCreate()
        {
            Categories = new List<Category>();
        }
    }
}