using System.ComponentModel.DataAnnotations;

namespace TestChatApp.Models
{
    public class FriendConnection
    {
        [Key]
        public int Id { get; set; }
        public int FirstUserId { get; set; }
        public int SecondUserId { get; set; }
        public virtual User FirstUser { get; set; }
        public virtual User SecondUser { get; set; }
    }
}