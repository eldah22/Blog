using SABlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SABlog.ViewModels
{
    public class CheckBox
    {
        public string Name { get; set; }
        public bool Selected { get; set; }

        public CheckBox(string n)
        {
            Name = n;
            Selected = false;
        }

        public CheckBox()
        {

        }
    }
}