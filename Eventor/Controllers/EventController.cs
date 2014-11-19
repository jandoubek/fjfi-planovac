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

        static readonly IEventRepository repository = new EventRepository();

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
