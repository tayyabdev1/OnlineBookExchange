using Microsoft.Owin.Security.Provider;
using OnlineBookExchange.DAL;
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
    [RoutePrefix ("api/UserVerification")]
    public class UserVerificationController : ApiController
    {
        OnlineBookExchangeEntities db;

        public UserVerificationController()
        {
            db = new OnlineBookExchangeEntities();
        }

        [HttpPost]
        [Route ("submitVerification")]
        public IHttpActionResult submitVerification(HttpRequestMessage request)
        {
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                try
                { 
                int UserId = Convert.ToInt32(httpRequest.Form["UserId"]);
                var CNICFront = httpRequest.Files["CNICFront"];
                var CNICBack = httpRequest.Files["CNICBack"];
                string CNICNumber = httpRequest.Form["CNICNumber"];

                string address = httpRequest.Form["Address"];
                string city = httpRequest.Form["City"];
                string postalcode = httpRequest.Form["PostalCode"];

                string frontPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/CNIC"), Guid.NewGuid() + Path.GetExtension(CNICFront.FileName));
                string backPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/CNIC"), Guid.NewGuid() + Path.GetExtension(CNICBack.FileName));
                CNICFront.SaveAs(frontPath);
                CNICBack.SaveAs(backPath);

                var verification = new UserVerifications
                {
                    UserId = UserId,
                    CNICNumber = CNICNumber,
                    CNICFrontImage = "/Images/CNIC/" + Path.GetFileName(frontPath),
                    CNICBackImage = "/Images/CNIC/" + Path.GetFileName(backPath),
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                };
                db.UserVerifications.Add(verification);

                var addressEntity = new UserAddresses
                {
                    UserId = UserId,
                    Address = address,
                    City = city,
                    PostalCode = postalcode,
                    VerificationStatus = "Pending",
                    CreatedAt = DateTime.Now,
                };
                db.UserAddresses.Add(addressEntity);
                db.SaveChanges();

                return Ok(new { message = "CNIC verification submitted successfully" });
            }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            return BadRequest("Required files are missing.");
        }
    }
}
