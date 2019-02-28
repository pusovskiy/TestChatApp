using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TestChatApp.Models;
using TestChatApp.ViewModels;

namespace TestChatApp.Controllers
{
    public class AccountController : Controller
    {
        
        public ActionResult List(string nameUser, int page = 1)
        {

            using (UserContext db = new UserContext())
            {
                var users = db.Users.ToList();

                var usersViewModels = new List<UserViewModel>();

                foreach (var user in users)
                {
                  usersViewModels.Add(new UserViewModel { Id = user.Id, Email = user.Email });
                }

                return View(usersViewModels);
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

        [HttpGet]
        public ActionResult AddFriend(int id)
        {
            FriendConnection connection = null;
            using (UserContext db = new UserContext())
            {
                connection = db.FriendConnections.FirstOrDefault(f =>
                    f.FirstUser.Email == User.Identity.Name && f.SecondUserId == id);

                if (connection != null)
                {
                    ModelState.AddModelError("", "You are friends");
                    ViewBag.Message = $"{User.Identity.Name} has user with this id in friend list";
                }
                else
                {
                    var user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
                    user.Friends.Add(new FriendConnection { FirstUserId = user.Id, SecondUserId = id });
                    db.SaveChanges();
                    ViewBag.Message = $"{User.Identity.Name} added new friend!";

                }

            }
            return View();
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("List", "Account");
        }
    }
}
