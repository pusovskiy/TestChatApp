using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
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
                        usersViewModels.Add(new UserViewModel {Id = user.Id, Email = user.Email});
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

        public static void AddToDatabase(string firstUserName, string secondUserName, string message)
        {
            using (UserContext db = new UserContext())
            {
                var firstUserId = db.Users.FirstOrDefault(u => u.Email == firstUserName).Id;
                var secondUserId = db.Users.FirstOrDefault(u => u.Email == secondUserName).Id;

                if (firstUserId != secondUserId)
                {
                    var dialogueConnect =
                        db.Dialogues.FirstOrDefault(d =>
                            d.FirstUserId == firstUserId && d.SecondUserId == secondUserId);

                    var dialogueConnectId = 0;
                    if (dialogueConnect == null)
                    {
                        dialogueConnect = db.Dialogues.FirstOrDefault(d =>
                            d.FirstUserId == secondUserId && d.SecondUserId == firstUserId);
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

        public ActionResult GetMessages(int id)
        {
            using (UserContext db = new UserContext())
            {
                var firstUserId = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name).Id;
                var secondUserId = id;


                var dialogConnection =
                    db.Dialogues.FirstOrDefault(d => d.FirstUserId == firstUserId && d.SecondUserId == secondUserId);

                var dialogId = 0;
                if (dialogConnection == null)
                {
                    dialogConnection = db.Dialogues.FirstOrDefault(d =>
                        d.FirstUserId == secondUserId && d.SecondUserId == firstUserId);
                    if (dialogConnection != null)
                    {
                        dialogId = dialogConnection.Id;
                    }
                }
                else
                {
                    dialogId = dialogConnection.Id;
                }

                if (dialogId != 0)
                {


                    var messages = db.Messages.Where(m => m.DialogueId == dialogId).ToList();

                    var messageViewModel = new List<MessagesViewModel>();

                    foreach (var message in messages)
                    {
                        messageViewModel.Add(new MessagesViewModel
                            {ChatMessage = message.ChatMessage, DateTime = message.DateTime.ToString()});
                    }

                    return Json(messageViewModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Dialogues.Add(new Dialogue {FirstUserId = firstUserId, SecondUserId = secondUserId});
                    db.SaveChanges();

                    return null;
                }

            }
        }
    }
}
