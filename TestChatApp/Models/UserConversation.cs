using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestChatApp.Models
{
    public class UserConversation
    {
        public int Id { get; set; }

        public User UserId { get; set; }

        public ICollection<Dialogue> Dialogues { get; set; }


    }
}