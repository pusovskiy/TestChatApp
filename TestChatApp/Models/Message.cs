using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int DialogueId { get; set; }

        public string ChatMessage { get; set; }

        public DateTime DateTime { get; set; }

        public virtual Dialogue Dialogue { get; set; }

        public Message()
        {
            DateTime = DateTime.UtcNow;
        }

        
    }
}