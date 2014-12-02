﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventor.Models
{
    public class ChatRepository
    {
        private static Dictionary<ChatUser, Guid> _connectedUsers;
        private static Dictionary<string, Guid> _mappings;
        private static ChatRepository _instance = null;

        public static ChatRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ChatRepository();
            }
            return _instance;
        }

        #region Private methods

        private ChatRepository()
        {
            _connectedUsers = new Dictionary<ChatUser, Guid>();
            _mappings = new Dictionary<string, Guid>();
        }

        #endregion

        #region Repository methods

        public IQueryable<KeyValuePair<ChatUser, Guid>> Users { get { return _connectedUsers.AsQueryable(); } }

        public void Add(ChatUser user, Guid eventId)
        {
            _connectedUsers.Add(user, eventId);
        }

        public void Remove(ChatUser user)
        {
            _connectedUsers.Remove(user);
        }

        public void AddMapping(string connectionId, Guid userId)
        {
            if (!string.IsNullOrEmpty(connectionId) && userId != Guid.Empty)
            {
                _mappings.Add(connectionId, userId);
            }
        }

        public Guid GetUserByConnectionId(string connectionId)
        {
            Guid userId;
            _mappings.TryGetValue(connectionId, out userId);
            return userId;
        }

        #endregion
    }
}