using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace OnlineBookExchange.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<int, string> ConnectedUsers = new Dictionary<int, string>();

        public override System.Threading.Tasks.Task OnConnected()
        {
            int userId = int.Parse(Context.QueryString["userId"]);
            if (!ConnectedUsers.ContainsKey(userId))
            {
                ConnectedUsers.Add(userId, Context.ConnectionId);
            }
            else
            {
                ConnectedUsers[userId] = Context.ConnectionId;
            }
            return base.OnConnected();
        }

        public static void SendPrivateMessage(int toUserId, string message)
        {
            if (ConnectedUsers.ContainsKey(toUserId))
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                context.Clients.Client(ConnectedUsers[toUserId]).receiveMessage(message);
            }
        }
    }
}