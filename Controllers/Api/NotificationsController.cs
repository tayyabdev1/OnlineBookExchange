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
            if (notificationsViewModel?.NotificationID == null)
            {
                return BadRequest("NotificationID is required.");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // We only need NotificationID to update status
                    var isUpdated = new NotificationDto().AcceptRequest(new NotificationDto
                    {
                        NotificationID = notificationsViewModel.NotificationID
                    });

                    if (isUpdated)
                    {
                        // Now create the exchange entry
                        var exchange = new Exchanges
                        {
                            BookID = notificationsViewModel.BookID,
                            OwnerID = notificationsViewModel.UserID,
                            RequestorID = notificationsViewModel.ReceiverId,
                            ExchangeDate = DateTime.Now,
                            ReturnDate = null,
                            Status = "In Progress",
                        };

                        _context.Exchanges.Add(exchange);
                        _context.SaveChanges();
                        transaction.Commit();

                        // Send notification email
                        var receiver = _context.Users.Find(notificationsViewModel.ReceiverId);
                        if (receiver != null)
                        {
                            string subject = "Your Request Accepted";
                            string body = $"Hello {receiver.Username}, <br><br> Your book request has been accepted. Please check your notifications.";
                            EmailHelper.SendEmail(receiver.Email, subject, body);
                        }

                        return Ok("Exchange request accepted.");
                    }
                    else
                    {
                        transaction.Rollback();
                        return StatusCode(HttpStatusCode.InternalServerError);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return InternalServerError(ex);
                }
            }
        }




        [HttpPost]
        [Route("api/notifications/decline")]
        public IHttpActionResult DeclineRequest(NotificationsViewModel notificationsViewModel)
        {
            if (notificationsViewModel?.NotificationID == null)
            {
                return BadRequest("NotificationID is required.");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Use DeclineRequest instead of AcceptRequest
                    var isUpdated = new NotificationDto().DeclineRequest(new NotificationDto
                    {
                        NotificationID = notificationsViewModel.NotificationID
                    });

                    if (isUpdated)
                    {
                        // Send notification email
                        var receiver = _context.Users.Find(notificationsViewModel.ReceiverId);
                        if (receiver != null)
                        {
                            string subject = "Your Request Declined";
                            string body = $"Hello {receiver.Username}, <br><br> Your book request has been Declined. Please check your notifications.";

                            EmailHelper.SendEmail(receiver.Email, subject, body);
                        }

                        transaction.Commit();
                        return Ok("Exchange request declined.");
                    }
                    else
                    {
                        transaction.Rollback();
                        return StatusCode(HttpStatusCode.InternalServerError);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return InternalServerError(ex);
                }
            }
        }
    }
}