using Eventor.App_Start;
using Eventor.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
            if (_eventRepository.GetAllEvents().Any())
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
        public ActionResult Detail(string EventId)
        {
            return View();
        
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

        // GET: Event/Chat
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Chat()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return View("Chat", "_EventLayout", new ChatUserViewModel(user));
        }

        // POST: Event/AddEventMember
        [HttpPost]
        public JsonResult AddEventMember(MemberShip newMembership)
        {
            if (_eventRepository.AddEventMember(newMembership))
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
