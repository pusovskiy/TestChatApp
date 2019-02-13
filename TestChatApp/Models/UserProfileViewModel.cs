using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace TestChatApp.Models
{
    public class UserProfileViewModel
    {
        public IEnumerable<User> Users { get; set; }

        public IEnumerable<FriendConnection> FriendConnections { get; set; }
    }
}