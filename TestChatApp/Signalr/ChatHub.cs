using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using TestChatApp.Models;
using TestChatApp.Signalr;

namespace TestChatApp
{
    public class ChatHub : Hub
    {   
        private static readonly ConnectionMapping<string> _connections = 
            new ConnectionMapping<string>();

        public void SendChatMessage(string who,string message)
        {
            string name = Context.User.Identity.Name;


            foreach (var connectionId in _connections.GetConnections(who))
            {
                Clients.Client(connectionId).messageRecieved(message);
            }

            TestChatApp.Controllers.ConversationController.AddToDatabase(name, who, message);

        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            
            _connections.Add(name,Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}