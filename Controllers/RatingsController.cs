using OnlineBookExchange.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBookExchange.Controllers { 

    public class RatingsController : Controller
    {
        // GET: Ratings
        public ActionResult Index()
        {
            return View();
        }
    }
}