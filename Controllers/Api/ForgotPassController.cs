using OnlineBookExchange.DAL;
using OnlineBookExchange.Helpers;
using OnlineBookExchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace OnlineBookExchange.Controllers.Api
{
    public class ForgotPassController : ApiController
    {
        [Route("api/ForgotPassword")]
        [HttpPost]
        public IHttpActionResult ForgotPassword(UserViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email))
            {
                return BadRequest("Invalid Data");
            }

            var PasswordHasher = new PasswordHasher();

            using (var db = new OnlineBookExchangeEntities())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Username == model.Username);
                if (user == null)
                {
                    return BadRequest("User not found");
                }

                string newPassword = GenerateRandomPassword();

                user.Password = PasswordHasher.HashPassword(newPassword);
                db.SaveChanges();

                if (model.Email != null)
                {
                    string subject = "Password Reset";
                    string body = $"Hello, Your new Password is: {newPassword}\n\n Please change it after Logging in.";


                    EmailHelper.SendEmail(model.Email, subject, body);
                }
                return Ok("Email Sent Succesfully");
            }
        }


        private string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%";
            Random random = new Random();
            return new string (Enumerable.Repeat(chars, length) .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        [Route ("api/ChangePassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword(UserViewModel model)
        {
            using (var db = new OnlineBookExchangeEntities())
            {
                var user = db.Users.FirstOrDefault(u => u.UserID == model.UserID);
                if (user == null)
                {
                    return BadRequest("User Not Found");
                }

                var PasswordHasher = new PasswordHasher();

                if (!IsValidPassword(model.Password))
                {
                    return BadRequest("Password must be at least 8 characters long, including an uppercase letter, a lowercase letter, a number, and a special character.");
                }

                if (PasswordHasher.VerifyHashedPassword(user.Password, model.CurrentPassword) == PasswordVerificationResult.Failed)
                {
                    return BadRequest("Current password is incorrect.");
                }
                if (model.NewPassword != model.ConfirmPassword)
                {
                    return BadRequest("New password and confirm password do not match.");
                }

                user.Password = PasswordHasher.HashPassword(model.NewPassword);
                db.SaveChanges();

                return Ok("Password changed successfully.");
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
