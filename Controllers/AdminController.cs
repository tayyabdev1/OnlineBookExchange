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
using System.Data.Entity;
using System.IO;

namespace OnlineBookExchange.Controllers
{
    [JwtAuthentication]
    public class AdminController : Controller
    {

        OnlineBookExchangeEntities db;

        public AdminController()
        {
            db = new OnlineBookExchangeEntities();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PendingCNIC()
        {
            var pendingList = (from uv in db.UserVerifications
                               join av in db.UserAddresses on uv.UserId equals av.UserId
                               where uv.Status == "Pending" || av.VerificationStatus == "Pending"
                               select new UserVerificationsViewModel
                               {
                                   UserId = uv.UserId,
                                   CNICNumber = uv.CNICNumber,
                                   CNICFrontImage = uv.CNICFrontImage,
                                   CNICBackImage = uv.CNICBackImage,
                                   CreatedAt = uv.CreatedAt,
                                   Address = av.Address,
                                   City = av.City,
                                   PostalCode = av.PostalCode,
                                   AddressCreatedAt = av.CreatedAt,
                               }).ToList();

            return View(pendingList);
        }
    }
}