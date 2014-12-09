using Eventor.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Eventor.Models
{
    public class EventViewModel
    {
        public EventViewModel()
        {
        }

        public EventViewModel(Event item)
        {
            this.EventId = item.EventId;

            this.Name = item.Name;
            this.Description = item.Description;
            this.Content = item.Content;
        }

        public Guid? EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }

    public class SubEventViewModel
    {
        public SubEventViewModel()
        {
        }

        public SubEventViewModel(SubEvent item)
        {
            this.SubEventId = item.SubEventId;

            this.Name = item.Name;
            this.Description = item.Description;
            this.Content = item.Content;
            this.ParentId = item.ParentId ?? Guid.Empty;
            this.EventId = item.EventId;
        }

        public Guid? SubEventId { get; set; }
        public Guid? ParentId { get; set; }
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }


    public class ChatUserViewModel
    {
        public ChatUserViewModel()
        {
        }

        public ChatUserViewModel(EventorUser item)
        {
            this.UserId = item.Id;

            this.UserName = item.UserName;
            this.FirstName = item.Name;
            this.LastName = item.Surname;

            this.Writing = false;
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Writing { get; set; }
    }

    public class ChatMessageViewModel
    {
        public ChatMessageViewModel()
        {
        }

        public ChatMessageViewModel(ChatMessage message)
        {
            this.User = new ChatUserViewModel(message.User);
            this.MessageId = message.MessageId;
            this.EventId = message.EventId;
            this.Content = message.Content;
            this.Timestamp = message.Timestamp;
        }

        public ChatUserViewModel User { get; set; }     
        public Guid MessageId { get; set; }
        public Guid EventId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class AutoCompleteUserViewModel
    {
        public AutoCompleteUserViewModel()
        {
        }

        public AutoCompleteUserViewModel(EventorUser item)
        {
            this.UserId = item.Id;

            this.UserName = item.UserName;
            this.FirstName = item.Name;
            this.LastName = item.Surname;
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EventConfirmationViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [BooleanRequired(ErrorMessage = "You must accept the terms and conditions.")]
        [Display(Name = "I accept the terms and conditions")]
        public bool iAgree { get; set; }
    }
    
    /// <summary>
    ///  Model for initial page used both for login and register either
    /// </summary>
    public class EventDetailViewModel
    {
        public EventConfirmationViewModel EventConfirmationViewModel { get; set; }
        public ChatUserViewModel ChatUserViewModel { get; set; }
    }
}
