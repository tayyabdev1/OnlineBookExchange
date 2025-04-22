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
        public string ProfilePicture { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }

        public virtual Users Users { get; set; }
        public virtual Users Users1 { get; set; }



    }
}
