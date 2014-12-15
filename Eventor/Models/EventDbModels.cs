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
        public virtual Guid EventId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public virtual string Name { get; set; }

        [Required]
        [DataType(DataType.Html)]
        public virtual string Description { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public virtual string Content { get; set; }

        public virtual ICollection<SubEvent> SubEvents { get; set; }
        public virtual ICollection<MemberShip> Memberships { get; set; }

        public Event()
        {
        }

        public Event(EventViewModel item)
        {
            this.EventId = item.EventId ?? Guid.Empty;

            this.Name = item.Name;
            this.Content = item.Content;
            this.Description = item.Description;
        }
    }

    public class SubEvent
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid SubEventId { get; set; }

        public virtual Guid EventId { get; set; }

        public virtual Guid? ParentId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public virtual string Name { get; set; }

        [Required]
        [DataType(DataType.Html)]
        public virtual string Description { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public virtual string Content { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }

        [ForeignKey("ParentId")]
        public SubEvent Parent { get; set; }

        public virtual ICollection<SubEvent> SubEvents { get; set; }
        public virtual ICollection<Asignee> Asignees { get; set; } 

        public SubEvent()
        {
        }

        public SubEvent(SubEventViewModel item)
        {
            this.EventId = item.EventId;
            this.SubEventId = item.SubEventId ?? Guid.Empty;
            this.ParentId = Guid.Equals(item.ParentId, Guid.Empty) ? null : item.ParentId;

            this.Name = item.Name;
            this.Content = item.Content;
            this.Description = item.Description;
        }
    }

    public class MemberShip
    {
        [Key]
        [Column(Order = 0)]
        public virtual Guid EventId { get; set; }

        [Key]
        [Column(Order = 1)]
        public virtual string UserId { get; set; }

        [DataType(DataType.Text)]
        public virtual string UserRole { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("UserId")]
        public virtual EventorUser User { get; set; }
    }

    public class Asignee
    {
        [Key]
        [Column(Order = 0)]
        public virtual string UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public virtual Guid SubEventId { get; set; }

        [ForeignKey("UserId")]
        public virtual EventorUser User { get; set; }

        [ForeignKey("SubEventId")]
        public virtual SubEvent SubEvent { get; set; }
    }

    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MessageId { get; set; }

        [Required]
        public virtual Guid EventId { get; set; }

        [Required]
        public virtual string UserId { get; set; }

        [DataType(DataType.Text)]
        public virtual string Content { get; set; }

        [DataType(DataType.DateTime)]
        public virtual DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        public virtual EventorUser User { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

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

