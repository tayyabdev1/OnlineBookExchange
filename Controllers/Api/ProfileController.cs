using OnlineBookExchange.BLL;
using OnlineBookExchange.DAL;
using OnlineBookExchange.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OnlineBookExchange.Controllers.Api
{
    public class ProfileController : ApiController
    {
        OnlineBookExchangeEntities db;

        public ProfileController()
        {
            db = new OnlineBookExchangeEntities();
        }

        [HttpGet]
        [Route("api/profile/{userID}")]
        public IHttpActionResult GetUser(int userID)
        {
            var user = new UserDto().GetUser(userID);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "User not found with ID: " + userID);
            }
        }


        // Upload Profile Picture
        [HttpPost]
        [Route("api/profile/UploadProfilePicture")]
        public IHttpActionResult UploadProfilePicture()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var imageFile = httpRequest.Files["imageFile"];

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string uploadFolder = HttpContext.Current.Server.MapPath("~/Images");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    imageFile.SaveAs(filePath);
                    try
                    {
                        // Save path in the database
                        int userId = Convert.ToInt32(httpRequest.Form["UserID"]); // Ensure UserID is sent in FormData
                        var user = db.Users.FirstOrDefault(u => u.UserID == userId);
                        if (user != null)
                        {
                            user.ProfilePicture = "/Images/" + uniqueFileName;
                            db.SaveChanges();
                        }
                        else
                        {
                            return BadRequest("User not found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                    return Ok(new { ProfilePicture = "/Images/" + uniqueFileName });
                }
            }

            return BadRequest("No file uploaded");
        }


        [HttpPut]
        public IHttpActionResult UpdateProfile(ViewModels.UserViewModel userViewModel)
        {
            if (userViewModel?.UserID == null)
            {
                return BadRequest("Invalid data.");
            }

            var updateUser = new UserDto().UpdateUser
            (
                new UserDto
                {
                    UserID = userViewModel.UserID,
                    Username = userViewModel.Username,
                    Email = userViewModel.Email,
                    Password = userViewModel.Password,
                    ContactNumber = userViewModel.ContactNumber,
                    Address = userViewModel.Address,
                }
            );

            if (updateUser) return Ok();
            else return BadRequest();
        }
    }
}
