using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eventor.Models
{
    public class EventorUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<EventorUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }

        public virtual ICollection<MemberShip> Memberships { get; set; }
        public virtual ICollection<Asignee> Asignees { get; set; }

        public EventorUser()
        {
        }

        public EventorUser(ChatUserViewModel user)
        {
            this.Id = user.UserId;

            this.UserName = user.UserName;
            this.Surname = user.LastName;
            this.Name = user.FirstName;
        }
    }
}
