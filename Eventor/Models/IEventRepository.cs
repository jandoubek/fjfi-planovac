using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventor.Models
{
    interface IEventRepository
    {
        IEnumerable<Event> getAllEvents();
        Event getEvent(int id);
        Event addEvent(Event item);
        Boolean removeEvent(string id);
        Boolean editEvent(Event item);
    }
}
