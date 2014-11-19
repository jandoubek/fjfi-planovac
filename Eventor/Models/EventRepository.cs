using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eventor.Models;
using System.Web.Helpers;

namespace Eventor.Models
{
    public class EventRepository :  Controller, IEventRepository
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

        public IEnumerable<Event> getAllEvents()
        {
            return db.Events.ToList();
        }

        public Event getEvent(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Event addEvent([Bind(Include = "EventID,Name,Description,Content")] Event @event)
        {
            if (ModelState.IsValid)
            {
                try 
                { 
                    db.Events.Add(@event);
                    db.SaveChanges();
                    return @event;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Boolean removeEvent(string id)
        {
            try 
            {
                Event @event = db.Events.Find(id);
                db.Events.Remove(@event);
                db.SaveChanges();
                return true;
            }
            catch
            { 
            return false;
            }
        }

        public bool editEvent([Bind(Include = "EventID,Name,Description,Content")] Event @event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Event @ToRemoveEvent = db.Events.Find(@event.EventID);
                    db.Events.Remove(@ToRemoveEvent);
                    db.Events.Add(@event);
                    db.SaveChanges();
                    return true;
                }
                return false;

            }
            catch
            {
                return false;
            }
        }
    }
}