using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBookExchange.Controllers
{
    [AllowAnonymous]
    public class SignupController : Controller
    {
        // GET: Signup
        public ActionResult Signup()
        {
            return View();
        }
    }
}