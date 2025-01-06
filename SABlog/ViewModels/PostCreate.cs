using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SABlog.Models;

namespace SABlog.ViewModels
{
    public class PostCreate
    {
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public List<CheckBox> SelectedCategories { get; set; }
        public List<PostCategory> SelectedCategoriess { get; set; }

        public List<Category> Categories { get; set; }

        public List<HttpPostedFileBase> UploadedFiles { get; set; }
        public List<PostFile> files { get; set; }


        public int UserId { get; set; }

        public PostCreate(List<Category> cats, List<CheckBox> cb, List<HttpPostedFileBase> f)
        {
            Categories = cats;
            SelectedCategories = cb;
            UploadedFiles = f;
        }

        public PostCreate()
        {
            Categories = new List<Category>();
            SelectedCategories = new List<CheckBox>();
            UploadedFiles = new List<HttpPostedFileBase>();
        }
    }
}