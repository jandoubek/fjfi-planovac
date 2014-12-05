using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Eventor.Models
{
    public class ChatRepository
    {
        private static Dictionary<EventorUser, Guid> _connectedUsers;
        private static Dictionary<string, string> _mappings;
        private static ChatRepository _instance = null;
        private static EventorDbContext _database;

        public static ChatRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ChatRepository();
            }
            return _instance;
        }

        private ChatRepository()
        {
            _connectedUsers = new Dictionary<EventorUser, Guid>();
            _mappings = new Dictionary<string, string>();
            _database = EventorDbContext.GetInstance();
        }

        public IQueryable<KeyValuePair<EventorUser, Guid>> Users { get { return _connectedUsers.AsQueryable(); } }
        public IQueryable<ChatMessage> MessageHistory { get { return _database.ChatMessages.AsQueryable();  } }

        public bool AddMessageToDatabase(ChatMessageViewModel message)
        {
            try
            {
                _database.ChatMessages.Add(new ChatMessage(message));
                _database.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Add(EventorUser user, Guid eventId)
        {
            _connectedUsers.Add(user, eventId);
        }

        public void Remove(EventorUser user)
        {
            _connectedUsers.Remove(user);
        }

        public void AddMapping(string connectionId, string userId)
        {
            if (!string.IsNullOrEmpty(connectionId) && userId != string.Empty)
            {
                _mappings.Add(connectionId, userId);
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            string userId;
            _mappings.TryGetValue(connectionId, out userId);
            return userId;
        }
    }
}