using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.Mvc;

namespace Eventor.Models
{
    public class Event
    {
        [Required]
        public string EventID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Html)]
        public string Description { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.Html)]
        public string Content { get; set; }
        public virtual ICollection<SubEvent> SubEvents { get; set; }
    }

    public class SubEvent
    {
        public string SubEventId { get; set; }
        public string ParentId { get; set; }
        public string Content { get; set; }
        public virtual ICollection<SubEvent> SubEvents { get; set; }
    }

    public class EventDbContext : DbContext
    {
        public EventDbContext()
            : base("EventDatabase")
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<SubEvent> SubEvents { get; set; }

    }
}
