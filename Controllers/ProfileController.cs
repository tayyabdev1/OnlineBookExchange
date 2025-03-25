using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineBookExchange.ViewModels;
using OnlineBookExchange.BLL;
using Microsoft.AspNet.Identity;
using OnlineBookExchange.DAL;
using System.IO;
using Microsoft.AspNetCore.Http;
using OnlineBookExchange.Services;

namespace OnlineBookExchange.Controllers
{
    public class ProfileController : Controller
    {
        OnlineBookExchangeEntities db;

        public ProfileController()
        {
            db = new OnlineBookExchangeEntities();
        }
        // GET: Profile
        public ActionResult Index()
        {
            int userId = GetCurrentUserId();

            // Fetch the user profile using the user ID
            var userDto = new OnlineBookExchange.BLL.UserDto();
            var profile = userDto.GetProfile(userId); // Pass the userId to GetProfile

            if (profile == null)
            {
                return HttpNotFound(); // Handle the case where the user is not found
            }

            // Create the UserViewModel and populate it with the profile data
            var userViewModel = new UserViewModel()
            {
                UserID = profile.UserID,
                Username = profile.Username,
                Email = profile.Email,
                ContactNumber = profile.ContactNumber,
                Address = profile.Address,
                Role = profile.Role,
                CreatedAt = profile.CreatedAt,
                ProfilePicture = profile.ProfilePicture,
            };

            return View(userViewModel);
        }


        // To see someone's Profile
        public ActionResult UserProfile()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            var userDto = new OnlineBookExchange.BLL.UserDto();
            var profile = userDto.GetProfile(userId);

            if (profile == null)
            {
                return HttpNotFound();
            }

            var userViewModel = new UserViewModel()
            {
                UserID = profile.UserID,
                Username = profile.Username,
                Email = profile.Email,
                ContactNumber = profile.ContactNumber,
                Address = profile.Address,
                Role = profile.Role,
                CreatedAt = profile.CreatedAt,
                ProfilePicture = profile.ProfilePicture,
            };

            return View(userViewModel);
        }


        public int GetCurrentUserId()
        {
            return Convert.ToInt32(Session["UserID"]);
        }

        
        public ActionResult MyBooks()
        {
            int userId = GetCurrentUserId();

            // Fetch the books uploaded by the user
            var booksDto = new OnlineBookExchange.BLL.BooksDto();
            var userBooks = booksDto.GetBooksByUserId(userId); // Implement this method in your BooksDto

            // Check if the user has uploaded books
            if (userBooks == null || !userBooks.Any())
            {
                ViewBag.Message = "You have not uploaded any books yet.";
                return View(new List<BooksViewModel>()); // Return an empty list
            }

            // Map the books to a view model
            var booksViewModel = userBooks.Select(book => new BooksViewModel
            {
                BookID = book.BookID,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                Condition = book.Condition,
                Status = book.Status,
                AddedDate = book.AddedDate
            }).ToList();

            return View(booksViewModel);
        }


        // Fetch Current User Profile picture
        public ActionResult GetProfilePicture()
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            // Retrieve profile picture from database
            var user = db.Users.Find(userID);
            string profilePicturePath = user?.ProfilePicture;

            // If user has no profile picture, set the default image path
            if (string.IsNullOrEmpty(profilePicturePath))
            {
                profilePicturePath = Server.MapPath("~/Images/default.png");
            }
            else
            {
                profilePicturePath = Server.MapPath(profilePicturePath);
            }

            return File(profilePicturePath, "image/jpeg");
        }


        public ActionResult AdminProfile()
        {
            int userId = GetCurrentUserId();

            // Fetch the user profile using the user ID
            var userDto = new OnlineBookExchange.BLL.UserDto();
            var profile = userDto.GetProfile(userId); // Pass the userId to GetProfile

            if (profile == null)
            {
                return HttpNotFound(); // Handle the case where the user is not found
            }

            // Create the UserViewModel and populate it with the profile data
            var userViewModel = new UserViewModel()
            {
                UserID = profile.UserID,
                Username = profile.Username,
                Email = profile.Email,
                ContactNumber = profile.ContactNumber,
                Address = profile.Address,
                Role = profile.Role,
                CreatedAt = profile.CreatedAt,
                ProfilePicture = profile.ProfilePicture,
            };

            return View(userViewModel);
        }
    }
}