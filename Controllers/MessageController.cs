using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineBookExchange.BLL;
using OnlineBookExchange.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Http.Results;
using OnlineBookExchange.DAL;
using OnlineBookExchange.Services;

namespace OnlineBookExchange.Controllers
{
    public class MessageController : Controller
    {
        OnlineBookExchangeEntities db;
        public MessageController()
        {
            db = new OnlineBookExchangeEntities();
            db.Configuration.ProxyCreationEnabled = false;

        }
        // GET: Message

        public ActionResult Message(int receiverId)
        {
            
            var userId = Convert.ToInt32(Session["UserID"]); // Current logged-in user

            var receiver = db.Users.FirstOrDefault(u => u.UserID == receiverId);
            string receiverName = receiver != null ? receiver.Username : "Unknown";

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

            var viewModel = new MessageViewModel
            {
                SenderID = userId,         // Current User
                ReceiverID = receiverId, // Chat Partner
                Receivername = receiverName,
                Messages = messages // Assign messages to the Messages property
            };

            return View(viewModel);
        }

        public ActionResult Index()
        {
            // Get the current logged-in user ID
            var userId = Convert.ToInt32(Session["UserID"]);

            // Get all messages where the user is either the sender or receiver
            var chats = db.Message
                .Where(m => m.SenderID == userId || m.ReceiverID == userId)
                .GroupBy(m => m.SenderID == userId ? m.ReceiverID : m.SenderID)  // Group by the other person
                .Select(group => new MessageViewModel
                {
                    SenderID = group.Key, // This will be the other user's ID
                    Message = group.OrderBy(m => m.CreatedAt).ToList()  // Order messages by timestamp
                })
                .ToList();

            return View(chats);  // Pass the chats to the view
        }


        public ActionResult ChatList()
        {
            int userId = Convert.ToInt32(Session["UserID"]); // Logged-in User

            var chatUsers = db.Message
                .Where(m => m.SenderID == userId || m.ReceiverID == userId)
                .Select(m => m.SenderID == userId ? m.ReceiverID : m.SenderID)
                .Distinct()
                .Join(db.Users, id => id, user => user.UserID, (id, user) => new MessageDto
                {
                    SenderID = user.UserID,
                    Username = user.Username
                })
                .ToList();

            return View(chatUsers);
        }


    }
}