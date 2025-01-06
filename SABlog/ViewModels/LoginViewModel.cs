using SABlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SABlog.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public CheckBox RememberMe { get; set; }

        public LoginViewModel(CheckBox c)
        {
            RememberMe = c;
        }

        public LoginViewModel() {
            RememberMe = new CheckBox();
        }
    }
}