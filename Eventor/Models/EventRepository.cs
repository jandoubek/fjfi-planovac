using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Eventor.Models
{
    public class EventRepository
    {
        private static EventRepository _instance = null;
        private static EventorDbContext db = EventorDbContext.GetInstance();

        public static EventRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EventRepository();
            }
            return _instance;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            try
            {
                var CurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                return db.MemberShips.Where(u => u.UserId == CurrentUserId).Select(u => u.Event).ToList();
            }
            catch
            {
                return new List<Event>();
            }
        }

        public bool IsAllowedToSee(Guid eventId, string userId)
        {
            return db.MemberShips.Where(u => u.EventId == eventId && u.UserId == userId).Any();
        }

        public IEnumerable<SubEvent> GetTasks(SubEvent item)
        {
            try
            {
                return db.SubEvents.Where(x => x.ParentId == item.SubEventId).ToList();
            }
            catch
            {
                return new List<SubEvent>();
            }
        }

        public IEnumerable<SubEvent> GetSubEvents(Event item)
        {
            try
            {
                return db.SubEvents.Where(x => x.EventId == item.EventId && !x.ParentId.HasValue).ToList();
            }
            catch
            {
                return new List<SubEvent>();
            }
        }

        public Event GetEvent(Guid EventId)
        {
            try
            {
                return db.Events.FirstOrDefault(x => x.EventId == EventId);
            }
            catch
            {
                return null;
            }
        }

        public bool AddEventMember(MemberShip newMembership)
        {
            try
            {
                db.MemberShips.Add(newMembership);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddEvent(ref Event item, string userId)
        {
            try
            {
                db.Events.Add(item);
                db.SaveChanges();

                db.MemberShips.Add(new MemberShip { EventId = item.EventId, UserId = userId, UserRole = "Admin" });
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddSubEvent(ref SubEvent item)
        {
            try
            {
                db.SubEvents.Add(item);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveEvent(Event item)
        {
            try 
            {
                Event itemToRemove = db.Events.FirstOrDefault(x => x.EventId == item.EventId);
                db.Events.Remove(itemToRemove);
                db.SaveChanges();
                return true;
            }
            catch
            { 
                return false;
            }
        }

        public bool RemoveSubEvent(SubEvent item)
        {
            try
            {
                SubEvent itemToRemove = db.SubEvents.FirstOrDefault(x => x.SubEventId == item.SubEventId);
                db.SubEvents.Remove(itemToRemove);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditEvent(Event item)
        {
            try
            {
                Event itemToUpdate = db.Events.FirstOrDefault(x => x.EventId == item.EventId);
                db.Entry(itemToUpdate).CurrentValues.SetValues(item);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditSubEvent(SubEvent item)
        {
            try
            {
                SubEvent itemToUpdate = db.SubEvents.FirstOrDefault(x => x.SubEventId == item.SubEventId);
                db.Entry(itemToUpdate).CurrentValues.SetValues(item);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
