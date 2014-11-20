using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.Mvc;

namespace Eventor.Models
{
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EventID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Html)]
        public string Description { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string Content { get; set; }

        public virtual ICollection<SubEvent> SubEvents { get; set; }
    }

    public class SubEvent
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        public string SubEventID { get; set; }

        public string ParentID { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string Content { get; set; }

        public virtual ICollection<SubEvent> SubEvents { get; set; }
    }

    public class MemberShip
    {
        [Key]
        [Column(Order = 0)]
        public string UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        public string EventID { get; set; }

        [DataType(DataType.Text)]
        public string UserRole { get; set; }
    }

    public class Asignee
    {
        [Key]
        [Column(Order = 0)]
        public string UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        public string SubEventID { get; set; }        
    }

    public class EventDbContext : DbContext
    {
        public EventDbContext()
            : base("EventDatabase")
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<SubEvent> SubEvents { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Asignee> Asignees { get; set; }
    }
}
