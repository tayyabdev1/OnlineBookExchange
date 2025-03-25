using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBookExchange.Controllers
{
    [AllowAnonymous]
    public class ForgotPassController : Controller
    {
        // GET: ForgotPass
        public ActionResult Index()
        {
            return View();
        }
    }
}