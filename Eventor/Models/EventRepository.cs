using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Eventor.Models
{
    public class EventRepository :  Controller
    {
        private static EventRepository _instance = null;
        private EventDbContext db = new EventDbContext();       

        public static EventRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EventRepository();
            }
            return _instance;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public IEnumerable<Event> GetAllEvents()
        {
            try
            {
                Guid UserID;
                Guid.TryParse(System.Web.HttpContext.Current.User.Identity.GetUserId(), out UserID);

                // Get only events which is available for the current user from membership table
                var match = from t1 in db.Events
                            join t2 in db.MemberShips on
                            t1.EventID equals t2.EventID
                            where t2.UserID == UserID
                            select t1;

                return match.ToList();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<SubEvent> GetTasks(SubEvent item)
        {
            try
            {
                return db.SubEvents.Where(x => x.SubEventID == item.SubEventID)
                    .SelectMany(x => x.SubEvents).ToList();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<SubEvent> GetSubEvents(Event item)
        {
            try
            {
                return db.Events.Where(x => x.EventID == item.EventID)
                    .SelectMany(x => x.SubEvents).ToList();
            }
            catch
            {
                return null;
            }
        }

        public Event GetEvent(Guid EventID)
        {
            try
            {
                return db.Events.FirstOrDefault(x => x.EventID == EventID);
            }
            catch
            {
                return null;
            }
        }

        public bool AddEventMember(Guid eventID, Guid userID, string role)
        {
            try
            {
                db.MemberShips.Add(new MemberShip { EventID = eventID, UserID = userID, UserRole = role });
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddEvent(ref Event item, Guid userID)
        {
            try
            {
                db.Events.Add(item);
                db.SaveChanges();

                db.MemberShips.Add(new MemberShip { EventID = item.EventID, UserID = userID, UserRole = "Admin" });
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
                Event itemToRemove = db.Events.FirstOrDefault(x => x.EventID == item.EventID);
                db.Events.Remove(itemToRemove);
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
                Event itemToUpdate = db.Events.FirstOrDefault(x => x.EventID == item.EventID);
                if (itemToUpdate != null)
                {
                    db.Entry(itemToUpdate).CurrentValues.SetValues(item);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }                
            }
            catch
            {
                return false;
            }
        }
    }
}