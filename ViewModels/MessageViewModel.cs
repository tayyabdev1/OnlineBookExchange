using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineBookExchange.BLL;
using OnlineBookExchange.DAL;

namespace OnlineBookExchange.ViewModels
{
    public class MessageViewModel
    {
        public List<MessageDto> Messages { get; set; }
        public List<Message> Message { get; set; }
        public int MessageID { get; set; }

        public string Receivername { get; set; }
        public Nullable<int> SenderID { get; set; }
        public Nullable<int> ReceiverID { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }

        public virtual Users Users { get; set; }
        public virtual Users Users1 { get; set; }
    }
}