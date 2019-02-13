using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestChatApp.Models;

namespace TestChatApp.Controllers
{
    public class ChatController : Controller
    {

        [HttpGet]
        public ActionResult AddFriend(User model, int id)
        {
            FriendConnection connection = null;
            using (UserContext db = new UserContext())
            {
                connection = db.FriendConnections.FirstOrDefault(f =>
                    f.FirstUser.Email == User.Identity.Name && f.SecondUserId == id);

                if(connection!=null)
                {
                    ModelState.AddModelError("","You are friends");
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

        public ActionResult CreateChatGroup(int chatFriendId)
        {   
           
            using (UserContext db = new UserContext())
            {
               var friendEmail = db.Users.FirstOrDefault(u => u.Id == chatFriendId).Email;
               ViewBag.FriendEmail = friendEmail;
            }
            return View();
        }

    }
}