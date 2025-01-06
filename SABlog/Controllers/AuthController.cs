using Microsoft.AspNet.Identity;
using SABlog.Models;
using SABlog.Repository;
using SABlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SABlog.Helpers;
using System.Net.Mail;

namespace SABlog.Controllers
{
    public class AuthController : BaseController
    {
        private IUserRepository _userRepository;
        private Helper helper;


        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            helper = new Helper();
        }

        public AuthController()
        {
            _userRepository = new UserRepository(new BlogDatabaseEntities1());
            helper = new Helper();
        }

        public ActionResult Index()
        {
            return Login();
        }

        public User GetLoggedInUser()
        {
            if (!User.Identity.IsAuthenticated) return null;
            return _userRepository.GetUserByEmail(User.Identity.Name);
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (TempData["Failed"] != null)
            {
                ViewBag.Failed = "Shtimi i Përdoruesit Deshtoi!";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Informacion i pa plotë!");

                if (_userRepository.checkUsername(user.UserName)) throw new Exception("Pseudonimi është i zënë!");
                if (_userRepository.checkEmail(user.Email)) throw new Exception("Emaili është i zënë!");

                _userRepository.AddUser(user);
                return createCookie(user, true);
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                TempData["Failed"] = "Deshtim";
                Debug.WriteLine(e.StackTrace);
            }

            return View(user);
        }

        [HttpGet]
        
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (TempData["Failed"] != null)
            {
                ViewBag.Failed = "Email/Pseudonim ose Fjalëkalim i Gabuar!";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
            try
            {
                var res = _userRepository.validateUser(user.Email, user.Password);
                if (res == null) throw new Exception("Email/Pseudonim ose Fjalëkalim i Gabuar!");
                return createCookie(res, user.RememberMe.Selected);
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                TempData["Failed"] = "Deshtim";
                Debug.WriteLine(e.Message);
            }

            return View(user);
        }


        private ActionResult createCookie(User user, bool rememberMe)
        {
            var Ticket = new FormsAuthenticationTicket(user.Email, true, 3000);
            string Encrypt = FormsAuthentication.Encrypt(Ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, Encrypt);
            if (rememberMe)
            {
                cookie.Expires = DateTime.Now.AddDays(30);
            }
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth");
        }

    
    }
}