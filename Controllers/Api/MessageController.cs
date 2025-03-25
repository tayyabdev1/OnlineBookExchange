using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using OnlineBookExchange.BLL;
using OnlineBookExchange.ViewModels;
using OnlineBookExchange.DAL;
using OnlineBookExchange.Helpers;
using OnlineBookExchange.Services;

namespace OnlineBookExchange.Controllers.Api
{
    public class MessageController : ApiController
    {
        OnlineBookExchangeEntities db;
        public MessageController()
        {
            db = new OnlineBookExchangeEntities();
            db.Configuration.ProxyCreationEnabled = false;

        }

        [HttpGet]
        [Route("api/message/history")]
        public IHttpActionResult GetChatHistory(int userId, int receiverId)
        {
            var messages = db.Message
                .Where(m => (m.SenderID == userId && m.ReceiverID == receiverId) ||
                            (m.SenderID == receiverId && m.ReceiverID == userId))
                .OrderBy(m => m.CreatedAt)
                .Select(m => new MessageDto
                {
                    SenderID = m.SenderID,
                    ReceiverID = m.ReceiverID,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt
                    
                })
                .ToList();

            return Ok(messages);
        }


        [HttpPost]
        [Route("api/message/send")]
        public IHttpActionResult SendMessage(MessageDto messageDto)
        {
            var message = new Message
            {
                SenderID = messageDto.SenderID,
                ReceiverID = messageDto.ReceiverID,
                Content = messageDto.Content,
                CreatedAt = DateTime.Now
            };

            db.Message.Add(message);
            db.SaveChanges();

            var receiver = db.Users.Find(messageDto.ReceiverID);
            if (receiver != null)
            {
                String subject = "New Message Notification";
                string body = $"Hello {receiver.Username},<br><br>You have received a new message:<br><br><i>{messageDto.Content}</i><br><br>Please log in to your account to reply.";
                try
                {
                    EmailHelper.SendEmail(receiver.Email, subject, body);
                }
                catch (Exception ex)
                {
                    return StatusCode(HttpStatusCode.InternalServerError);
                }
            }
            return Ok("Message sent successfully");
        }


        [HttpGet]
        [Route("api/message/chatlist")]
        public IHttpActionResult GetChatList(int userId)
        {
            var chatUsers = db.Message
                .Where(m => m.SenderID == userId || m.ReceiverID == userId)
                .Select(m => m.SenderID == userId ? m.ReceiverID : m.SenderID) // Get other user's ID
                .Distinct()
                .Join(db.Users, id => id, user => user.UserID, (id, user) => new MessageDto
                {
                    SenderID = user.UserID,
                    Username = user.Username
                })
                .ToList();

            return Ok(chatUsers);
        }
    }
}
