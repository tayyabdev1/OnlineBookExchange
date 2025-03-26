using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using OnlineBookExchange.DAL;

namespace OnlineBookExchange.BLL
{
    public class NotificationDto
    {
        OnlineBookExchangeEntities db;
        public NotificationDto()
        {
            db = new OnlineBookExchangeEntities();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public int NotificationID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Message { get; set; }
        public Nullable<bool> ReadStatus { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public int ReceiverId { get; set; }
        public string Status { get; set; }
        public Nullable<int> BookID { get; set; }
        public Nullable<bool> IsHandled { get; set; }
        public BooksDto Books { get; set; }

        public virtual Users Users { get; set; }
    

        public bool SendNotification(NotificationDto notificationDto)
        {
            var newNotification = new Notifications
            {
                UserID = notificationDto.UserID,
                Message = notificationDto.Message,
                ReadStatus = notificationDto.ReadStatus,
                CreatedAt = DateTime.Now,
                ReceiverId = notificationDto.ReceiverId,
                Status = "Pending",
                BookID = notificationDto.BookID,
                IsHandled = notificationDto.IsHandled
            };
            db.Notifications.Add(newNotification);
            var savedChanges = db.SaveChanges();
            return savedChanges > 0;
        }

        public bool AcceptRequest(NotificationDto notificationDto)
        {
            var existingNotification = db.Notifications.FirstOrDefault(n => n.NotificationID == notificationDto.NotificationID);

            if (existingNotification != null)
            {
                existingNotification.Status = "Accepted";
                existingNotification.Message = "Accepted your exchange request for ";
                existingNotification.ReadStatus = true;
                existingNotification.IsHandled = true;

                return db.SaveChanges() > 0;
            }
            return false;
        }



        public bool DeclineRequest(NotificationDto notificationDto)
        {
            var existingNotification = db.Notifications.FirstOrDefault(n => n.NotificationID == notificationDto.NotificationID);

            if (existingNotification != null)
            {
                existingNotification.Status = "Declined";
                existingNotification.Message = "Your exchange request has been declined.";
                existingNotification.ReadStatus = true;
                existingNotification.IsHandled = true;

                return db.SaveChanges() > 0;
            }
            return false;
        }


        public Users GetUserById(int userId)
        {
            return db.Users.FirstOrDefault(u => u.UserID == userId);
        }

    }
}
