using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineBookExchange.ViewModels;
using OnlineBookExchange.DAL;
using OnlineBookExchange.BLL;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.AspNet.Identity;
using OnlineBookExchange.Services;
using OnlineBookExchange.Helpers;
using System.Net.Mail;
using System.Net;

namespace OnlineBookExchange.Controllers
{
    public class UserVerificationController : Controller
    {
        OnlineBookExchangeEntities db;

        public UserVerificationController()
        {
            db = new OnlineBookExchangeEntities();
        }

        public ActionResult SubmitAddress(string Address, string City, string PostalCode)
        {
            if (!string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(PostalCode))
            {
                var addressVerification = new UserAddresses
                {
                    UserId = GetCurrentUserId(),
                    Address = Address,
                    City = City,
                    PostalCode = PostalCode,
                    VerificationStatus = "Pending"
                };
                db.UserAddresses.Add(addressVerification);
                db.SaveChanges();
                return RedirectToAction("VerificationPending");
            }
            return View("Error");
        }


        public int GetCurrentUserId()
        {
            return Convert.ToInt32(Session["UserID"]);
        }


        [HttpPost]
        public JsonResult Approve(int userId, string status, UserVerificationsViewModel user)
        {
            var cnicVerification = db.UserVerifications.FirstOrDefault(v => v.UserId == userId && v.Status == "Pending");
            var addressVerification = db.UserAddresses.FirstOrDefault(v => v.UserId == userId && v.VerificationStatus == "Pending");
            var userVerified = db.Users.FirstOrDefault(v => v.UserID == userId);

            if (cnicVerification != null || addressVerification != null)
            {
                if (cnicVerification != null)
                {
                    cnicVerification.Status = status;
                }
                if (addressVerification != null)
                {
                    addressVerification.VerificationStatus = status;
                }
                if (userVerified != null)
                {
                    userVerified.IsVerified = true;
                }

                db.SaveChanges();

                if (addressVerification != null)
                {
                    var receiver = db.Users.Find(user.UserId);
                    if (receiver != null)
                    {
                        string subject = $"Verification Request";
                        string body = $"Hello {receiver.Username}, Your Verification request is {addressVerification.VerificationStatus}";
                        try
                        {
                            EmailHelper.SendEmail(receiver.Email, subject, body);
                        }
                        catch (Exception ex)
                        {
                            return Json(new { success = false, message = "Error sending email: " + ex.Message });
                        }
                    }
                    return Json(new { success = true, message = "Email sent successfully." });
                }
            return Json(new { success = true, message = $"CNIC status updated to {status}." });
            }
            return Json(new { success = false, message = "Verification not found or already processed." });
        }


        [HttpPost]
        public JsonResult Reject(int userId, string status, string reason, UserVerificationsViewModel user)
        {
            var cnicVerification = db.UserVerifications.FirstOrDefault(v => v.UserId == userId && v.Status == "Pending");
            var addressVerification = db.UserAddresses.FirstOrDefault(v => v.UserId == userId && v.VerificationStatus == "Pending");
            var userVerified = db.Users.FirstOrDefault(v => v.UserID == userId);

            if (cnicVerification != null || addressVerification != null)
            {
                if (cnicVerification != null)
                {
                    cnicVerification.Status = status;
                }
                if (addressVerification != null)
                {
                    addressVerification.VerificationStatus = status;
                }
                if (userVerified != null)
                {
                    userVerified.IsVerified = false;
                }

                db.SaveChanges();

                if (addressVerification != null)
                {
                    var receiver = db.Users.Find(user.UserId);
                    if (receiver != null)
                    {
                        string subject = $"Verification Request";
                        string body = $"Hello {receiver.Username}, Your Verification request is {addressVerification.VerificationStatus}\n\n Reason:\n\n{reason}";
                        try
                        {
                            EmailHelper.SendEmail(receiver.Email, subject, body);
                        }
                        catch (Exception ex)
                        {
                            return Json(new { success = false, message = "Error sending email: " + ex.Message });
                        }
                    }
                    return Json(new { success = true, message = "Email sent successfully." });
                }
                return Json(new { success = true, message = $"CNIC status updated to {status}." });
            }
            return Json(new { success = false, message = "Verification not found or already processed." });
        }

    }
}