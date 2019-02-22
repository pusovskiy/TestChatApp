using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestChatApp.Models;

namespace TestChatApp.Controllers
{
    public class ConversationController : Controller
    {
        // GET: Conversation
        public ActionResult Index()
        {
            using (UserContext db = new UserContext())
            {
                return View(db.Users.ToList());
            }

        }

        public void Chat(int secondUserId)
        {
            Dialogue dialogue = null;
            using (UserContext db = new UserContext())
            {
                var firstUserId = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;

                dialogue = db.Dialogues.FirstOrDefault(d =>
                    d.FirstUserId == firstUserId && d.SecondUserId == secondUserId);

                ViewBag.SecondUserName = db.Users.FirstOrDefault(u => u.Id == secondUserId).Email;

                if (dialogue == null)
                {
                    db.Dialogues.Add(new Dialogue {FirstUserId = firstUserId, SecondUserId = secondUserId});

                }
            }
        }
    }
}
