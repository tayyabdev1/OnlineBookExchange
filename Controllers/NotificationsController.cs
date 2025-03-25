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

            var notifications = _context.Notifications
                .Where(n => n.ReceiverId == userId)
                .Select(n => new NotificationsViewModel
                {
                    NotificationID = n.NotificationID,
                    UserID = n.UserID,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    ReceiverId = n.ReceiverId,
                    ReadStatus = n.ReadStatus,
                    Status = n.Status,
                    BookID = n.BookID,
                    IsHandled = n.IsHandled,
                    //Title = n.Book.Title, // Assuming Book entity is related
                })
                .ToList();

            return View(notifications);
        }
    }
}