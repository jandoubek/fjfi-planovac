using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Eventor.Models
{
    public class EventRepository :  Controller
    {
        private EventDbContext db = new EventDbContext();

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

        [HttpPost]
        public Event AddEvent(Event item)
        {
            try 
            { 
                Guid UserID;
                Guid.TryParse(System.Web.HttpContext.Current.User.Identity.GetUserId(), out UserID);

                db.Events.Add(item);
                db.SaveChanges();

                db.MemberShips.Add(new MemberShip { EventID = item.EventID, UserID = UserID, UserRole = "Kokot" });
                db.SaveChanges();

                return item;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
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

        [HttpPost]
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