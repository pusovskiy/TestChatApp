using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestChatApp.Models
{
    public class Dialogue
    {
        public int Id { get; set; }

        public User FirstUserId { get; set; }

        public User SecondUserId { get; set; }

        public string Text { get; set; }

        public DateTime DateOfStart { get; set; }
        
        public virtual User FirstUser { get; set; }

        public virtual User SecondUser { get; set; }
    }
}