using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eventor.Models;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;

namespace Eventor.Controllers
{
    public class EventController : Controller
    {
        private ChatRepository _chatRepository;
        private EventRepository _eventRepository;
        private EventorUserDbContext _userDatabase;

        public EventController()
        {
            _chatRepository = ChatRepository.GetInstance();
            _eventRepository = EventRepository.GetInstance();
            _userDatabase = EventorUserDbContext.GetInstance();
        }

        // GET: /Event/Index
        [HttpGet]
        [Authorize(Roles = "Registred")]
        public ActionResult Index()
        {
            // If no present events in list: Display Tutorial
            if (_eventRepository.GetAllEvents().Count() == 0)
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
        [Authorize(Roles = "Registred")]

        public ActionResult Overview()
        {
            return View();
        }

        // GET: /Event/Tutorial
        [HttpGet]
        [Authorize(Roles = "Registred")]
        public ActionResult Tutorial()
        {
            return View();
        }
        
        // GET: /Event/Detail/5        
        [HttpGet]
        [Authorize(Roles = "Visitor, Registred")]
        [Route("~/Event/Detail/{EventName}/{EventID}")] 
        public ActionResult Detail(string EventID)
        {
            return View();
        }

        // GET: /Event/Create       
        [HttpGet]
        public ActionResult Create()
        {
            return View("Detail");
        }

        // POST: /Event/GetEvent
        [HttpPost]
        public JsonResult GetEvent([Bind(Include = "EventID")] Event item)
        {
            return Json(_eventRepository.GetEvent(item.EventID), JsonRequestBehavior.DenyGet);
        }

        // POST: /Event/GetAllEvents
        [HttpPost]
        public JsonResult GetAllEvents()
        {
            return Json(_eventRepository.GetAllEvents(), JsonRequestBehavior.DenyGet);
        }

        // POST: /Event/GetSubEvents
        [HttpPost]
        public JsonResult GetSubEvents([Bind(Include = "EventID, Name, Description, Content")] Event item)
        {
            return Json(_eventRepository.GetSubEvents(item), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult AddEvent([Bind(Include = "Name, Description")] Event item)
        {
            Guid UserID;
            Guid.TryParse(User.Identity.GetUserId(), out UserID);

            if (_eventRepository.AddEvent(ref item, UserID))
            {
                return Json(item, JsonRequestBehavior.DenyGet);
            }
            else
            { 
                return null;
            }
        }

        [HttpPost]
        public JsonResult RemoveEvent([Bind(Include = "EventID, Name, Description, Content")] Event item)
        {
            if (_eventRepository.RemoveEvent(item))
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
            if(_eventRepository.EditEvent(item))
            {
                return Json(new { Status = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { Status = false }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Chat()
        {
            EventorUser user = _userDatabase.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            return View("Chat", "_EventLayout", new ChatUser() { UserName = user.UserName, UserId = Guid.Parse(user.Id), FirstName = user.Name, LastName = user.Surname} );
        }
    }
}
