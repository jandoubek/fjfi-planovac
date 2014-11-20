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
                return db.Events.ToList();
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
                db.Events.Add(item);
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