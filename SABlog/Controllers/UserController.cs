using SABlog.Models;
using SABlog.Repository;
using SABlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SABlog.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserController()
        {
            _userRepository = new UserRepository(new BlogDatabaseEntities1());
        }

        public ActionResult Index()
        {
            return RedirectToAction("UserProfile", "User");
        }

        public ActionResult UserProfile(string username)
        {
            User user;
            if (username == null || username == "") user = _userRepository.GetUserByEmail(User.Identity.Name);
            else user = _userRepository.GetUserByUsername(username);
            if (user == null) return RedirectToAction("Index", "Home");
            return View("~/Views/User/Profile.cshtml", user);
        }

        [HttpGet]
        public ActionResult Update()
        {
            User user = _userRepository.GetUserByEmail(User.Identity.Name);
            if (user == null) return RedirectToAction("Index", "Home");

            if (TempData["Failed"] != null)
            {
                ViewBag.Failed = TempData["Failed"];
            }
            return View("~/Views/User/UpdateProfile.cshtml", user);
        }

        [HttpPost]
        public ActionResult Update(User user)
        {
            try
            {
                User currentUser = _userRepository.GetUserByEmail(User.Identity.Name);
                if (!ModelState.IsValid) throw new Exception("Informacion i pa plote!");

                if(user.Email != currentUser.Email)
                {
                    if (_userRepository.checkEmail(user.Email)) throw new Exception("Email eshte i zene!");
                }
                if(user.UserName != currentUser.UserName)
                {
                    if (_userRepository.checkUsername(user.UserName)) throw new Exception("Pseudonimi eshte i zene!");
                }
                if (_userRepository.validateUser(currentUser.Email, user.Password) == null)
                {
                    throw new Exception("Fjalëkalimi Aktual nuk është i saktë!");
                }

                _userRepository.UpdateUser(currentUser, user);
                return RedirectToRoute("Profile", new { username = user.UserName });

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                TempData["Failed"] = e.Message;
                Console.WriteLine(e.Message);
                return Update();
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult UpdatePassword(string currentPassword, string newPassword)
        {
            try
            {
                User currentUser = _userRepository.GetUserByEmail(User.Identity.Name);
                if (!ModelState.IsValid) throw new Exception("Informacion i pa plote!");
                if (_userRepository.validateUser(currentUser.Email, currentPassword) == null)
                {
                    throw new Exception("Fjalëkalimi Aktual nuk është i saktë!");
                }

                _userRepository.UpdatePassword(currentUser, newPassword);
                return RedirectToRoute("Profile", new { username = user.UserName });
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                TempData["Failed"] = e.Message;
                Console.WriteLine(e.Message);
                return Update();
            }

            return RedirectToAction("Index", "Home");
        }

    }
}