using Eventor.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Eventor.Models
{
    public class EventConfirmationViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
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
    }
}
