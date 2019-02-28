using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestChatApp.Models;
using TestChatApp.ViewModels;

namespace TestChatApp.Controllers
{
    public class ConversationController : Controller
    {
        // GET: Conversation
        public ActionResult Index()
        {
            using (UserContext db = new UserContext())
            {
                var users = db.Users.ToList();

                var usersViewModels = new List<UserViewModel>();

                foreach (var user in users)
                {
                    if (user.Email != User.Identity.Name)
                    {
                        usersViewModels.Add(new UserViewModel { Email = user.Email });
                    }
                }

                //var usersViewModels = users.Select(x => 
                //        new UserViewModel
                //        {
                //            Email = x.Email
                //        }).Where()
                //    .ToList();

                return View(usersViewModels);
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

        public static void AddToDatabase(string firstUserName,string secondUserName,string message)
        {
            using (UserContext db = new UserContext())
            {
                var firstUserId = db.Users.FirstOrDefault(u => u.Email == firstUserName).Id;
                var secondUserId = db.Users.FirstOrDefault(u => u.Email == secondUserName).Id;

                if (firstUserId != secondUserId) { 
                var dialogueConnect =
                    db.Dialogues.FirstOrDefault(d => d.FirstUserId == firstUserId && d.SecondUserId == secondUserId);

                var dialogueConnectId = 0;
                if (dialogueConnect == null)
                {
                    dialogueConnect = db.Dialogues.FirstOrDefault(d => d.FirstUserId == secondUserId && d.SecondUserId == firstUserId);
                    if (dialogueConnect == null)
                    {
                        db.Dialogues.Add(new Dialogue {FirstUserId = firstUserId, SecondUserId = secondUserId});
                        db.SaveChanges();
                        dialogueConnectId = db.Dialogues
                            .FirstOrDefault(d => d.FirstUserId == firstUserId && d.SecondUserId == secondUserId).Id;
                        }
                    else
                    {
                        dialogueConnectId = db.Dialogues
                            .FirstOrDefault(d => d.FirstUserId == secondUserId && d.SecondUserId == firstUserId).Id;
                    }

                }
                else
                {
                    dialogueConnectId = db.Dialogues
                        .FirstOrDefault(d => d.FirstUserId == firstUserId && d.SecondUserId == secondUserId).Id;
                }

                db.Messages.Add(new Message {DialogueId = dialogueConnectId, ChatMessage = message});
                db.SaveChanges();
                }
            }
        }
    }
}
