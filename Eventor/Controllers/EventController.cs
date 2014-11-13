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

namespace Eventor.Controllers
{
    public class EventController : Controller
    {
        private EventDbContext db = new EventDbContext();

        static readonly IEventRepository repository = new EventRepository();

        // GET: /Event/
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: /Event/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: /Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EventID,Name,Description,Content")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: /Event/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: /Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="EventID,Name,Description,Content")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: /Event/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: /Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //--Petr--added Actions

        public ActionResult Overview()
        {
            return View();
        }

        public JsonResult GetAllEvents()
        {
            return Json(repository.getAllEvents(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddEvent(Event item)
        {
            Event newEvent = repository.addEvent(item);
            if (newEvent != null)
            {
                return Json(newEvent, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(newEvent, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RemoveEvent(Event item)
        {
            if (repository.removeEvent(item.EventID))
            {
                return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EditEvent(Event item)
        {
            if(repository.editEvent(item))
            {
                return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = false }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
