using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineBookExchange.BLL;
using OnlineBookExchange.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Http.Results;
using OnlineBookExchange.DAL;
using OnlineBookExchange.Services;

namespace OnlineBookExchange.Controllers
{
    [JwtAuthentication]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}