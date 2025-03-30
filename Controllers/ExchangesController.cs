using OnlineBookExchange.DAL;
using OnlineBookExchange.Services;
using OnlineBookExchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace OnlineBookExchange.Controllers
{
    public class ExchangesController : Controller
    {
        private readonly OnlineBookExchangeEntities _context;

        public ExchangesController()
        {
            _context = new OnlineBookExchangeEntities();
        }
        // GET: Exchanges
        public ActionResult Index()
        {
            int currentUser = Convert.ToInt32(Session["UserID"]);
            var exchanges = (from e in _context.Exchanges
                             join b in _context.Books on e.BookID equals b.BookID
                             where e.OwnerID == currentUser || e.RequestorID == currentUser
                             select new ExchangesViewModel
                {
                    ExchangeID = e.ExchangeID,
                    BookID = e.BookID,
                    Title = b.Title,
                    OwnerID = e.OwnerID,
                    RequestorID = e.RequestorID,
                    Status = e.Status,
                    ExchangeDate = e.ExchangeDate,
                    ReturnDate = e.ReturnDate,
                    Ratings = e.Ratings,
                }).ToList();
            return View(exchanges);
        }

        public ActionResult AdminView()
        {
            var exchanges = (from ex in _context.Exchanges
                            join b in _context.Books on ex.BookID equals b.BookID
                            select new ExchangesViewModel
            {
                ExchangeID = ex.ExchangeID,
                BookID = ex.BookID,
                Title = b.Title,
                OwnerID = ex.OwnerID,
                RequestorID = ex.RequestorID,
                Status = ex.Status,
                Ratings = ex.Ratings,
                ExchangeDate = ex.ExchangeDate,
                ReturnDate = ex.ReturnDate
            }).ToList();
            return View(exchanges);
        }


        public JsonResult GetUnreadCount()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            int count = _context.Notifications.Count(n => n.ReceiverId == userId && n.ReadStatus == false);
            return Json(count, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult MarkAsRead()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            var notifications = _context.Notifications.Where(n => n.ReceiverId == userId && n.ReadStatus == false).ToList();

            foreach (var notification in notifications)
            {
                notification.ReadStatus = true;
            }

            _context.SaveChanges();
            return new HttpStatusCodeResult(200);
        }
    }
}