using OnlineBookExchange.DAL;
using OnlineBookExchange.Services;
using OnlineBookExchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBookExchange.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly OnlineBookExchangeEntities _context;

        public NotificationsController()
        {
            _context = new OnlineBookExchangeEntities();
        }
        // GET: Notification
        public ActionResult Index()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            var notifications = (from n in _context.Notifications
                                 join u in _context.Users on n.UserID equals u.UserID
                                 join b in _context.Books on n.BookID equals b.BookID
                                 where n.ReceiverId == userId
                                 select new NotificationsViewModel
                {
                    NotificationID = n.NotificationID,
                    UserID = n.UserID,
                    Username = u.Username,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    ReceiverId = n.ReceiverId,
                    ReadStatus = n.ReadStatus,
                    Status = n.Status,
                    BookID = n.BookID,
                    Title = b.Title,
                    IsHandled = n.IsHandled,
                    //Title = n.Book.Title, // Assuming Book entity is related
                })
                .ToList();

            return View(notifications);
        }
    }
}