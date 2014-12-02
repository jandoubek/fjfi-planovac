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
        public Guid SubEventID { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string Content { get; set; }

        public virtual ICollection<SubEvent> SubEvents { get; set; }
    }

    public class MemberShip
    {
        [Key]
        [Column(Order = 0)]
        public Guid UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid EventID { get; set; }

        [DataType(DataType.Text)]
        public string UserRole { get; set; }
    }

    public class Asignee
    {
        [Key]
        [Column(Order = 0)]
        public Guid UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid SubEventID { get; set; }        
    }

    public class EventDbContext : DbContext
    {
        private static EventDbContext _instance = null;

        public EventDbContext()
            : base("EventDatabase")
        {
        }

        public static EventDbContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EventDbContext();
            }
            return _instance;
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<SubEvent> SubEvents { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Asignee> Asignees { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
