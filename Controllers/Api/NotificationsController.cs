using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineBookExchange.DAL;
using OnlineBookExchange.BLL;
using OnlineBookExchange.ViewModels;
using System.Net.Mail;
using OnlineBookExchange.Helpers;
using OnlineBookExchange.Services;


namespace OnlineBookExchange.Controllers.Api
{
    
    public class NotificationsController : ApiController
    {
        private readonly OnlineBookExchangeEntities _context;

        public NotificationsController()
        {
            _context = new OnlineBookExchangeEntities();
        }

        [HttpPost]
        [Route("api/notifications/send")]
        public IHttpActionResult SendNotification(NotificationsViewModel notificationsViewModel)
        {
            if (notificationsViewModel.UserID == null)
            {
                return BadRequest("Notification data is required.");
            }
            var notifications = new NotificationDto().SendNotification
                (
                    new NotificationDto
                    {
                        UserID = notificationsViewModel.UserID,
                        Message = notificationsViewModel.Message,
                        ReadStatus = false,
                        CreatedAt = DateTime.Now,
                        ReceiverId = notificationsViewModel.ReceiverId,
                        Status = "Pending",
                        BookID = notificationsViewModel.BookID,
                        IsHandled = false,
                    }
                );
            if (notifications)
            {
                var receiver = _context.Users.Find(notificationsViewModel.ReceiverId);
                if (receiver != null)
                {
                    string subject = "New Exchange Request Notification";
                    string body = $"Hello {receiver.Username},<br><br>You have received a new exchange request for your book. Please check your notifications.";

                    EmailHelper.SendEmail(receiver.Email, subject, body);
                }
                    return Ok("Email sent successfully.");
                }
            else return StatusCode(HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        [Route("api/notifications/accept")]
        public IHttpActionResult AcceptRequest(NotificationsViewModel notificationsViewModel)
        {
            if (notificationsViewModel?.UserID == null)
            {
                return BadRequest("NotificationID is required.");
            }
            var notifications = new NotificationDto().AcceptRequest
                (
                    new NotificationDto
                    {
                        UserID = notificationsViewModel.UserID,
                        Message = notificationsViewModel.Message,
                        ReadStatus = true,
                        CreatedAt = DateTime.Now,
                        ReceiverId = notificationsViewModel.ReceiverId,
                        Status = "Accepted",
                        BookID = notificationsViewModel.BookID,
                        IsHandled = true,

                    }
                );
            if (notifications)
            {
                var exchange = new Exchanges
                {
                    BookID = notificationsViewModel.BookID,
                    OwnerID = notificationsViewModel.UserID,
                    RequestorID = notificationsViewModel.ReceiverId,
                    ExchangeDate = DateTime.Now,
                    ReturnDate = null,
                    Status = "In Progress",
                };
                _context.Exchanges.Add( exchange );
                _context.SaveChanges();

                var receiver = _context.Users.Find(notificationsViewModel.ReceiverId);
                if (receiver != null)
                {
                    string subject = "Your Request Accept";
                    string body = $"Hello {receiver.Username}, <br><br> Your book request has been accepted. Please check your notifications.";

                    EmailHelper.SendEmail(receiver.Email, subject, body);
                }
                return Ok("Email sent successfully");
            }
            else return StatusCode(HttpStatusCode.InternalServerError);
        }
        
        [HttpPost]
        [Route("api/notifications/decline")]
        public IHttpActionResult declineRequest(NotificationsViewModel notificationsViewModel)
        {
            if (notificationsViewModel.UserID == null)
            {
                return BadRequest("Notification data is required.");
            }
            var notifications = new NotificationDto().DeclineRequest
                (
                    new NotificationDto
                    {
                        UserID = notificationsViewModel.UserID,
                        Message = notificationsViewModel.Message,
                        ReadStatus = true,
                        CreatedAt = DateTime.Now,
                        ReceiverId = notificationsViewModel.ReceiverId,
                        Status = "Declined",
                        BookID = notificationsViewModel.BookID,
                        IsHandled = true,
                    }
                );
            if (notifications)
            {
                var receiver = _context.Users.Find(notificationsViewModel.ReceiverId);
                if(receiver != null)
                {
                    string subject = "Your Book request Declined";
                    string body = $"Hello {receiver.Username}, <br><br> Your book request declined. Please check your Notifications";

                    EmailHelper.SendEmail(receiver.Email, subject, body);
                }
                return Ok("Email sent succesfully");
            }
            else return StatusCode(HttpStatusCode.InternalServerError);
        }
    }
}