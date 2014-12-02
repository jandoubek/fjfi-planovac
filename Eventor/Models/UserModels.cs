using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eventor.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
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
    }

    public class EventorUserDbContext : IdentityDbContext<EventorUser>
    {
        private static EventorUserDbContext _instance = null;

        public EventorUserDbContext()
            : base("UserDatabase", throwIfV1Schema:false)
        {
        }

        public static EventorUserDbContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EventorUserDbContext();
            }
            return _instance;
        }

        public static EventorUserDbContext Create()
        {
            return new EventorUserDbContext();
        }
    }
}
