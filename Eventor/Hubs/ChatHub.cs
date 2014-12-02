using Eventor.Models;
using Eventor.Utilities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Linq;

namespace Eventor.Hubs
{
    public class ChatHub : Hub
    {
        private ChatRepository _repository;

        public ChatHub()
        {
            _repository = ChatRepository.GetInstance();
        }

        #region IDisconnect and IConnected event handlers implementation

        /// <summary>
        /// Fired when a client disconnects from the system. The user associated with the client ID gets deleted from the list of currently connected users.
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            Guid userId = _repository.GetUserByConnectionId(Context.ConnectionId);
            if (userId != null)
            {
                ChatUser user = _repository.Users.Where(u => u.UserId == userId).FirstOrDefault();
                if (user != null)
                {
                    _repository.Remove(user);
                    return Clients.All.leaves(user, DateTime.Now);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        #endregion

        #region Chat event handlers

        /// <summary>
        /// Fired when a client pushes a message to the server.
        /// </summary>
        /// <param name="message"></param>
        public void Send(ChatMessage message)
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                // Sanitize input
                message.Content = HttpUtility.HtmlEncode(message.Content);
                // Process URLs: Extract any URL and process rich content (e.g. Youtube links)
                HashSet<string> extractedURLs;
                message.Content = TextParser.TransformAndExtractUrls(message.Content, out extractedURLs);
                message.Timestamp = DateTime.Now;
                Clients.All.onMessageReceived(message);
            }
        }

        /// <summary>
        /// Fired when a client joins the chat. Here round trip state is available and we can register the user in the list
        /// </summary>
        public void Joined(ChatUser user)
        {
            // Do not add user to contact list wheather is already present
            if (!_repository.Users.Where(m => m.UserId == user.UserId).Any())
            {
                _repository.Add(user);
                _repository.AddMapping(Context.ConnectionId, user.UserId);
                Clients.All.joins(user, DateTime.Now);
            }            
        }

        /// <summary>
        /// Invoked when a client connects. Retrieves the list of all currently connected users
        /// </summary>
        /// <returns></returns>
        public ICollection<ChatUser> GetConnectedUsers()
        {
            return _repository.Users.ToList<ChatUser>();
        }

        #endregion
    }
}