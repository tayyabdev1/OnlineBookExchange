using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineBookExchange.ViewModels;
using System.Net;
using System.Web.Security;
using OnlineBookExchange.BLL;
using OnlineBookExchange.Services;
using OnlineBookExchange.DAL;
using Microsoft.AspNet.Identity;

namespace OnlineBookExchange.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private OnlineBookExchangeEntities db = new OnlineBookExchangeEntities();
        // GET: Login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
            var username = user.Username;
            var password = user.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Please enter a valid username and password");
                return View(user);
            }

            var appUserInfo = db.Users.FirstOrDefault(u => u.Username == username);

            if (appUserInfo == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(user);
            }

            var passwordHasher = new PasswordHasher();

            if (passwordHasher.VerifyHashedPassword(appUserInfo.Password, password) != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(user);
            }

            // User authenticated - proceed with login
            var role = appUserInfo.Role;
            var jwtToken = Authentication.GenerateJWTAuthetication(username, role);
            var validUserName = Authentication.ValidateToken(jwtToken);

            if (string.IsNullOrEmpty(validUserName))
            {
                ModelState.AddModelError("", "Unauthorized login attempt");
                return View(user);
            }

            var cookie = new HttpCookie("jwt", jwtToken)
            {
                HttpOnly = true,
                Secure = false, // Change to true if using HTTPS
            };
            Response.Cookies.Add(cookie);

            Session["UserID"] = appUserInfo.UserID.ToString();
            Session["UserName"] = appUserInfo.Username;
            Session["Role"] = role;

            if (role.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AdminLoggedIn", "Login");
            }

            return RedirectToAction("LoggedIn", "Login");
        }



        public ActionResult LoggedIn()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Books");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult AdminLoggedIn()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //public ActionResult Login(LoginViewModel lvm)
        //{
        //    var userDto = new UserDto();
        //    var user = userDto.GetUserId(lvm.Username, lvm.Password); // Assuming this returns a user object or null.

        //    if (user == null)
        //    {
        //        ModelState.AddModelError("", "Invalid Username or Password");
        //        return View();
        //    }

        //    if (user.IsActive != true)
        //    {
        //        ModelState.AddModelError("", "Your account is inactive. Please contact support.");
        //        return View();
        //    }

        //    // Set session and role-based redirection
        //    Session["UserID"] = user.UserID;
        //    var role = user.Role;
        //    if (role == "admin")
        //    {
        //        HttpCookie cookie = new HttpCookie("access", "admin");
        //        Response.Cookies.Add(cookie);
        //        cookie.Expires = DateTime.Now.AddMinutes(120);
        //        FormsAuthentication.SetAuthCookie(lvm.Username, false);
        //        return RedirectToAction("Index", "Admin");
        //    }
        //    else if (role == "User")
        //    {
        //        HttpCookie cookie = new HttpCookie("access", "User");
        //        cookie.HttpOnly = true;
        //        cookie.Secure = Request.IsSecureConnection;
        //        cookie.Expires = DateTime.Now.AddMinutes(120);
        //        Response.Cookies.Add(cookie);
        //        FormsAuthentication.SetAuthCookie(lvm.Username, false);
        //        return RedirectToAction("Index", "Books");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Invalid UserName and Password");
        //        return View();
        //    }
        //}

        [HttpPost]
        public ActionResult Logout()
        {
            var cookie = new HttpCookie("jwt", "")
            {
                Expires = DateTime.Now.AddYears(-1),
                HttpOnly = true
            };
            Response.Cookies.Set(cookie);

            // Abandon the session
            Session.Abandon();

            return RedirectToAction("Login", "Login");
        }
    }
}