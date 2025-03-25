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
    [AllowAnonymous]
    public class BooksController : Controller
    {
        OnlineBookExchangeEntities db;

        public BooksController()
        {
            db = new OnlineBookExchangeEntities();
        }
        public ActionResult Index()
        {
            var vm = new ViewModels.BooksViewModel
            {
                UserID = Convert.ToInt32(Session["UserID"])
            };
            vm.books = new BooksDto().GetBooks();
            return View(vm);
        }

        // Fetch Current User Profile picture
        public ActionResult GetBookPicture(int bookID)
        {
            // Retrieve profile picture from database
            var book = db.Books.Find(bookID);
            string imagePath = book?.BookPicture;

            // If user has no profile picture, set the default image path
            if (string.IsNullOrEmpty(imagePath) || !System.IO.File.Exists(Server.MapPath(imagePath)))
            {
                imagePath = "~/Images/BookDefault.jpg";
            }

            return File(Server.MapPath(imagePath), "image/jpeg");
        }
        public ActionResult AdminBookView()
        {

            var vm = new ViewModels.BooksViewModel
            {
                UserID = Convert.ToInt32(Session["UserID"]),
                book = new BooksDto().GetBooks()
            };
            return View(vm);
        }

        public ActionResult Reviews(int bookId)
        {
            var ratingsDto = new RatingsDto();
            var dtoReviews = ratingsDto.GetBookReviews(bookId);

            if (dtoReviews == null || !dtoReviews.Any())
            {
                ViewBag.Message = "No reviews available for this Book.";
                return View(new List<RatingsViewModel>());
            }

            var reviews = dtoReviews.Select(dto => new RatingsViewModel
            {
                Exchange_id = dto.Exchange_id,
                RaterID = dto.RaterID,
                RateeID = dto.RateeID,
                BookID = dto.BookID,
                BookConditionRating = dto.BookConditionRating,
                ExchangePartnerRating = dto.ExchangePartnerRating,
                Rview = dto.Rview,
                CreatedAt = dto.CreatedAt
            }).ToList();

            return View(reviews);
        }
    }
}