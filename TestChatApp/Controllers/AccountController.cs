using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TestChatApp.Models;

namespace TestChatApp.Controllers
{
    public class AccountController : Controller
    {
        
        public ActionResult List(string nameUser, int page = 1)
        {

            using (UserContext db = new UserContext())
            {

                return View(db.Users.ToList());
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("List", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public string Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == model.Email);
                }

                if (user == null)
                {

                    using (UserContext db = new UserContext())
                    {
                        db.Users.Add(new User { Email = model.Email, Password = model.Password });
                        db.SaveChanges();

                        FormsAuthentication.SetAuthCookie(model.Email, true);
                        //return RedirectToAction("List", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                    return "This user already exists";
                }

                return "successfully registered";
            }

                return "Passwords doesn't match";
            }

        [HttpGet]
        public ActionResult UserProfile()
        {
            var friends = new List<User>();
            using (UserContext db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                friends = user.Friends.Select(x => x.SecondUser).ToList();
            }
            
            return View(friends);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("List", "Account");
        }
    }
}
