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
            var exchanges = _context.Exchanges
                .Where(e => e.OwnerID == currentUser || e.RequestorID == currentUser)
                .Select(e => new ExchangesViewModel
            {
                ExchangeID = e.ExchangeID,
                BookID = e.BookID,
                OwnerID = e.OwnerID,
                RequestorID = e.RequestorID,
                Status = e.Status,
                Ratings = e.Ratings,
            }).ToList();
            return View(exchanges);
        }

        public ActionResult AdminView()
        {
            var exchanges = _context.Exchanges
                .Select(ex => new ExchangesViewModel
            {
                ExchangeID = ex.ExchangeID,
                BookID = ex.BookID,
                OwnerID = ex.OwnerID,
                RequestorID = ex.RequestorID,
                Status = ex.Status,
                Ratings = ex.Ratings,
            }).ToList();
            return View(exchanges);
        }
    }
}