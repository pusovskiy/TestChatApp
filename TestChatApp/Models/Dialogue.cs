using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestChatApp.Models
{
    public class Dialogue
    {
        public int Id { get; set; }

        public int FirstUserId { get; set; }

        public int SecondUserId { get; set; }
       
        public virtual User FirstUser { get; set; }

        public virtual User SecondUser { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

    }

}