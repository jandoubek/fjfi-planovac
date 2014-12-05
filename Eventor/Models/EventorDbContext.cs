using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Eventor.Models
{
    public class EventorDbContext : IdentityDbContext<EventorUser>
    {
        private static EventorDbContext _instance = null;

        public EventorDbContext()
            : base("EventorDatabase")
        {
        }

        public static EventorDbContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EventorDbContext();
            }
            return _instance;
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<SubEvent> SubEvents { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Asignee> Asignees { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}