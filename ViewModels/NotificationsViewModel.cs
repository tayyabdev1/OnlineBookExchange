using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineBookExchange.BLL;
using OnlineBookExchange.DAL;

namespace OnlineBookExchange.ViewModels
{
    public class NotificationsViewModel
    {
        public int NotificationID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Message { get; set; }
        public Nullable<bool> ReadStatus { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public int ReceiverId { get; set; }
        public string Status { get; set; }
        public Nullable<int> BookID { get; set; }
        public Nullable<bool> IsHandled { get; set; }

        public virtual Users Users { get; set; }
    }
}