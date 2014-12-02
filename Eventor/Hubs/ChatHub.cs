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

        public override Task OnDisconnected(bool stopCalled)
        {
            Guid userId = _repository.GetUserByConnectionId(Context.ConnectionId);
            if (userId != null)
            {
                KeyValuePair<ChatUser, Guid> connectionInfo = _repository.Users.Where(m => m.Key.UserId == userId).FirstOrDefault();
                ChatUser user = connectionInfo.Key;
                Guid eventId = connectionInfo.Value;

                if (user != null)
                {
                    _repository.Remove(user);
                    Groups.Remove(Context.ConnectionId, eventId.ToString());
                    Clients.Group(eventId.ToString()).leaves(user, DateTime.Now);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

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
                
                // Save the message to the database
                _repository.AddMessageToDatabase(message);
                // Notice members on a message received
                Clients.Group(message.EventId.ToString()).onMessageReceived(message);
            }
        }

        public void Joined(ChatUser user, Guid eventId)
        {
            _repository.Add(user, eventId);
            _repository.AddMapping(Context.ConnectionId, user.UserId);

            // Subscribe user to the event room
            Groups.Add(Context.ConnectionId, eventId.ToString()).Wait();

            // Notice members about user connection
            Clients.Group(eventId.ToString()).joins(user, DateTime.Now);
        }

        /// <summary>
        /// Invoked when a client connects. Retrieves the list of all currently connected users
        /// </summary>
        /// <returns></returns>
        public ICollection<ChatUser> GetConnectedUsers(Guid eventId)
        {
            // Get the list of online members by eventid
            return _repository.Users.Where(m => m.Value == eventId).Select(m => m.Key).ToList();
        }

        public ICollection<ChatMessage> GetMessageHistory(Guid eventId)
        {
            // Get the list of message history by eventid
            return _repository.MessageHistory.Where(u => u.EventId == eventId).ToList();           
        }
    }
}