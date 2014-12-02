using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eventor.Models
{
    public class ChatUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
    }

    public class ChatMessage
    {
        public ChatUser User { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}