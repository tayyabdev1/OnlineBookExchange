using MediaBrowser.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using OnlineBookExchange.BLL;
using Microsoft.AspNet.Identity;
using OnlineBookExchange.DAL;
using OnlineBookExchange.ViewModels;
using OnlineBookExchange.Helpers;

namespace OnlineBookExchange.Controllers.Api
{
    public class SignupController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Signup(UserViewModel userViewModel)
        {
            if (string.IsNullOrEmpty(userViewModel.Users.Username))
            {
                return BadRequest("Provide valid Name");
            }

            if (!IsValidPassword(userViewModel.Users.Password))
            {
                return BadRequest("Password must be at least 8 characters long, including an uppercase letter, a lowercase letter, a number, and a special character.");
            }

            using (var db = new OnlineBookExchangeEntities())
            {

                var existingUser = db.Users.Any(u => u.Username == userViewModel.Users.Username);
                var exisitingEmail = db.Users.Any(u => u.Email == userViewModel.Users.Email);
                if (existingUser)
                {
                    return Content(HttpStatusCode.Conflict, "This Username already exists, try another one.");
                }
                if (exisitingEmail)
                {
                    return Content(HttpStatusCode.Conflict, "This Email already registered, try another one.");
                }

                var passwordHasher = new PasswordHasher();
                string hashedPassword = passwordHasher.HashPassword(userViewModel.Users.Password); // Hash the password

                var newUser = new Users
                {
                    Username = userViewModel.Users.Username,
                    Email = userViewModel.Users.Email,
                    Password = hashedPassword,
                    ContactNumber = userViewModel.Users.ContactNumber,
                    Address = userViewModel.Users.Address,
                    Role = "User",
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                };

            
                db.Users.Add(newUser);
                int saveChanges = db.SaveChanges();

                if (saveChanges > 0)
                {
                    string subject = "Welcome to Online Book Exchange";
                    string body = $"Hello, {userViewModel.Username} Welcome to Online Book Exchange.";

                    EmailHelper.SendEmail(userViewModel.Email, subject, body);

                    return Created("", newUser);
                }
                else
                {
                    return BadRequest("Signup failed, Please try again.");
                }
            }
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");

            return regex.IsMatch(password);
        }
    }
}
