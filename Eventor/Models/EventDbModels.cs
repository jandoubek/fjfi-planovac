using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Eventor.Models
{
    public class Event
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EventId { get; set; }

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
        public virtual ICollection<MemberShip> Memberships { get; set; }

        public Event()
        {
        }

        public Event(EventViewModel item)
        {
            this.EventId = item.EventId;

            this.Name = item.Name;
            this.Content = item.Content;
            this.Description = item.Description;
        }
    }

    public class SubEvent
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        public Guid SubEventId { get; set; }

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
        public virtual ICollection<Asignee> Asignees { get; set; } 
    }

    public class MemberShip
    {
        [Key]
        [Column(Order = 0)]
        public Guid EventId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }

        [DataType(DataType.Text)]
        public string UserRole { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("UserId")]
        public virtual EventorUser User { get; set; }
    }

    public class Asignee
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid SubEventId { get; set; }

        [ForeignKey("UserId")]
        public EventorUser User { get; set; }

        [ForeignKey("SubEventId")]
        public SubEvent SubEvent { get; set; }
    }

    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MessageId { get; set; }

        [Required]
        public Guid EventId { get; set; }

        [Required]
        public string UserId { get; set; }

        [DataType(DataType.Text)]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        public EventorUser User { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }

        public ChatMessage()
        {
        }

        public ChatMessage(ChatMessageViewModel mesage)
        {
            this.EventId = mesage.EventId;
            this.UserId = mesage.User.UserId;

            this.Content = mesage.Content;
            this.Timestamp = mesage.Timestamp;            
        }
    }
}

