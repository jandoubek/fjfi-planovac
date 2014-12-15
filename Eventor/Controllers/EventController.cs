﻿using Eventor.App_Start;
using Eventor.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Eventor.Helpers;

namespace Eventor.Controllers
{
    public class EventController : Controller
    {
        private EventorUserManager _userManager;
        private EventRepository _eventRepository;        

        public EventController()
        {
            _eventRepository = EventRepository.GetInstance();
        }

        public EventorUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<EventorUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: /Event/Index
        [HttpGet]
        [Authorize(Roles = "Registred")]
        public ActionResult Index()
        {
            // If no present events in list: Display Tutorial
            if (!_eventRepository.GetAllEvents().Any())
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

        [HttpGet]
        [Authorize(Roles = "Registred")]
        public ActionResult Detail2()
        {
            return View();
        }

        [HttpGet]
        public ActionResult detail3()
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
        [Route("~/Event/Detail/{EventName}/{EventId}")] 
        public async Task<ActionResult> Detail(Guid EventId)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (!_eventRepository.IsAllowedToSee(EventId, user.Id))
            {
                return RedirectToAction("Overview");
            }
            
            var model = new EventDetailViewModel() { ChatUserViewModel = new ChatUserViewModel(user), EventConfirmationViewModel = null };
            return View(model);
        
        }

        // GET: /Event/AutoComplete
        public ActionResult AutoComplete()
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
        public JsonResult GetEvent(Guid eventId)
        {
            Event @event = _eventRepository.GetEvent(eventId);
            EventViewModel model = new EventViewModel(@event);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        // POST: /Event/GetEvents
        [HttpPost]
        public JsonResult GetEvents()
        {
            IEnumerable<Event> events = _eventRepository.GetAllEvents();
            var model = events.Select(u => new EventViewModel(u)).ToList();
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        // POST: /Event/GetSubEvents
        [HttpPost]
        public JsonResult GetSubEvents([Bind(Include = "EventId, Name, Description, Content")] EventViewModel item)
        {
            IEnumerable<SubEvent> subEvents = _eventRepository.GetSubEvents(new Event(item));
            var model = subEvents.Select(u => new SubEventViewModel(u)).ToList();
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        // POST: /Event/GetSubEvents
        [HttpPost]
        public JsonResult GetAllSubEvents([Bind(Include = "EventId, Name, Description, Content")] EventViewModel item)
        {
            IEnumerable<SubEvent> subEvents = _eventRepository.GetAllSubEvents(new Event(item));
            var model = subEvents.Select(u => new SubEventViewModel(u)).ToList();
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        // POST: /Event/AddEvent
        [HttpPost]
        public JsonResult AddEvent([Bind(Include = "Name, Description")] EventViewModel item)
        {
            Event @event = new Event(item);
            var currentUser = User.Identity.GetUserId();

            if (_eventRepository.AddEvent(ref @event, currentUser))
            {
                return Json(new EventViewModel(@event), JsonRequestBehavior.DenyGet);
            }
            else
            { 
                return null;
            }
        }

        // POST: /Event/AddSubEvent
        [HttpPost]
        public JsonResult AddSubEvent(SubEventViewModel item)
        {
            SubEvent subEvent = new SubEvent(item);

            if (_eventRepository.AddSubEvent(ref subEvent))
            {
                return Json(new SubEventViewModel(subEvent), JsonRequestBehavior.DenyGet);
            }
            else
            {
                return null;
            }
        }

        // POST: /Event/RemoveSubEvent
        [HttpPost]
        public JsonResult RemoveSubEvent(SubEventViewModel item)
        {
            if (_eventRepository.RemoveSubEvent(new SubEvent(item)))
            {
                return Json(new { Status = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { Status = false }, JsonRequestBehavior.DenyGet);
            }
        }

        // POST: /Event/RemoveEvent
        [HttpPost]
        public JsonResult RemoveEvent([Bind(Include = "EventId, Name, Description, Content")] EventViewModel item)
        {
            if (_eventRepository.RemoveEvent(new Event(item)))
            {
                return Json(new { Status = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { Status = false }, JsonRequestBehavior.DenyGet);
            }
        }

        // POST: Event/EditEvent
        [HttpPost]
        public JsonResult EditEvent([Bind(Include = "EventId, Name, Description, Content")] EventViewModel item)
        {
            if(_eventRepository.EditEvent(new Event(item)))
            {
                return Json(new { Status = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { Status = false }, JsonRequestBehavior.DenyGet);
            }
        }

        // POST: Event/EditSubEvent
        [HttpPost]
        public JsonResult EditSubEvent(SubEventViewModel item)
        {
            if (_eventRepository.EditSubEvent(new SubEvent(item)))
            {
                return Json(new { Status = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { Status = false }, JsonRequestBehavior.DenyGet);
            }
        }

        // POST: Event/GetAllMembers
        [HttpPost]
        public JsonResult GetAllMembers(SubEventViewModel item)
        {
            var model = _eventRepository.GetAllMembers(item.EventId);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        // POST: Event/AddMember
        [HttpPost]
        public async Task<JsonResult> AddMember(Guid EventId, string UserId, string UserRole)
        {
            MemberShip membership = new MemberShip() { EventId = EventId, UserId = UserId, UserRole = UserRole };
            
            if (_eventRepository.AddEventMember(membership))
            {
                EventorUser user = await UserManager.FindByIdAsync(UserId);
                Event @event = _eventRepository.GetEvent(EventId);

                var body = "Hello " + user.Name + " " + user.Surname + "<br />";
                body += "you have been invited to participate event called " + @event.Name + ". ";
                body += "Feel free to join it via the following link: ";
                var link = "http://eventor.cz/Event/Detail/" + @event.Name.ToSeoUrl() + "/" + @event.EventId;
                body += "<a href=\"" + link + "\">" + link + "</a><br />";
                body += "<img src=\"http://eventor.cz/Content/img/logo_120.png\"><br />";
                body += "Eventor - Be ready to your event<br />";
                body += "<a href=\"http://eventor.cz\">http://eventor.cz</a>";                
                //await UserManager.SendEmailAsync(UserId, "Eventor: You have been invited to event", body);

                return Json(new { Status = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { Status = false }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}
