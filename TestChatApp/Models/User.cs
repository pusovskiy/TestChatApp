using System.Collections.Generic;

namespace TestChatApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<FriendConnection> Friends { get; set; }

        public virtual ICollection<Dialogue> Dialogues { get; set; }
    }
}