using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookExchange.BLL
{
    public class MessageDto
    {
        OnlineBookExchangeEntities db;
        public MessageDto()
        {
            db = new OnlineBookExchangeEntities();
            db.Configuration.ProxyCreationEnabled = false;

        }
        public int MessageID { get; set; }
        public string Username { get; set; }
        public Nullable<int> SenderID { get; set; }
        public Nullable<int> ReceiverID { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }

        public virtual Users Users { get; set; }
        public virtual Users Users1 { get; set; }


        //public MessageDto GetChatHistory(int msgID)
        //{
        //    var msgs = (from m in db.Message
        //      join u in db.Users on m.SenderID equals u.UserID
        //      where m.MessageID == msgID
        //      select new MessageDto
        //      {
        //          MessageID = m.MessageID,
        //          Username = u.Username,
        //          SenderID = m.SenderID,
        //          ReceiverID = m.ReceiverID,
        //          Content = m.Content,
        //      }).FirstOrDefault();
        //    return msgs;
        //}
    }
}
