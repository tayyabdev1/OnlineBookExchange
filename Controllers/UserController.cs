using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineBookExchange.ViewModels;
using OnlineBookExchange.BLL;
using System.Data.Entity;
using OnlineBookExchange.Services;

// This controller is use for "View Profile" function
namespace OnlineBookExchange.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private OnlineBookExchangeEntities db = new OnlineBookExchangeEntities();
        // GET: User

        public ActionResult Profile(int id)
        {
            // Fetch the user from the database
            var user = db.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null)
            {
                return HttpNotFound("User not found!");
            }

            var userViewModel = new UserViewModel
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                ContactNumber = user.ContactNumber,
                Address = user.Address,
                CreatedAt = user.CreatedAt,
                ProfilePicture = user.ProfilePicture
            };

            // Pass the ViewModel to the view
            return View(userViewModel);
        }

        public ActionResult Reviews(int userId)
        {
            var ratingsDto = new RatingsDto();
            var dtoReviews = ratingsDto.GetUserReviews(userId);

            if (dtoReviews == null || !dtoReviews.Any())
            {
                ViewBag.Message = "No reviews available for this user.";
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