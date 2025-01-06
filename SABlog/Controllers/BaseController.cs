using SABlog.Models;
using SABlog.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SABlog.Controllers
{
    public abstract class BaseController : Controller
    {
        public User user;
        private IUserRepository _userRepository;

        public BaseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            user = _userRepository.GetUserByEmail(System.Web.HttpContext.Current.User.Identity.Name);
            ViewBag.LoggedInUser = user;
        }

        public BaseController()
        {
            _userRepository = new UserRepository(new BlogDatabaseEntities1());
            user = _userRepository.GetUserByEmail(System.Web.HttpContext.Current.User.Identity.Name);
            ViewBag.LoggedInUser = user;
        }


    }
}