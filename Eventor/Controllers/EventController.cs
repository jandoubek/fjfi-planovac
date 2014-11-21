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

        static readonly EventRepository repository = new EventRepository();
        
        // GET: /Event/Index
        [HttpGet]
        public ActionResult Index()
        {
            // If no present events in list: Display Tutorial
            if (repository.GetAllEvents().Count() == 0)
            {
                return RedirectToAction("Tutorial");
            }
            else // Otherways: Display Overview
            {
                return RedirectToAction("Overview");
            }
        }

        // GET: /Event/Overview
        [HttpGet]
        public ActionResult Overview()
        {
            return View();
        }

        // GET: /Event/Tutorial
        [HttpGet]
        public ActionResult Tutorial()
        {
            return View();
        }

        // GET: /Event/Detail/5
        [Route("~/Event/Detail/{EventName}/{EventID}")] 
        [HttpGet]
        public ActionResult Detail(string EventID)
        {
            return View();
        }

        // POST: /Event/GetEvent
        [HttpPost]
        public JsonResult GetEvent([Bind(Include = "EventID")] Event item)
        {
            return Json(repository.GetEvent(item.EventID), JsonRequestBehavior.DenyGet);
        }

        // POST: /Event/GetAllEvents
        [HttpPost]
        public JsonResult GetAllEvents()
        {
            return Json(repository.GetAllEvents(), JsonRequestBehavior.DenyGet);
        }

        // POST: /Event/GetSubEvents
        [HttpPost]
        public JsonResult GetSubEvents([Bind(Include = "EventID, Name, Description, Content")] Event item)
        {
            return Json(repository.GetSubEvents(item), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult AddEvent([Bind(Include = "Name, Description")] Event item)
        {
            Event newEvent = repository.AddEvent(item);
            if (newEvent != null)
            {
                return Json(newEvent, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(newEvent, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult RemoveEvent([Bind(Include = "EventID, Name, Description, Content")] Event item)
        {
            if (repository.RemoveEvent(item))
            {
                return Json(new { Status = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { Status = false }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult EditEvent([Bind(Include = "EventID, Name, Description, Content")] Event item)
        {
            if(repository.EditEvent(item))
            {
                return Json(new { Status = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { Status = false }, JsonRequestBehavior.DenyGet);
            }
        }

    }
}
