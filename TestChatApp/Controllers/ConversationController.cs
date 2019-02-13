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
    }
}