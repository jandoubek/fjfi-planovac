using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Eventor.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class EventorUser : IdentityUser
    {
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
    }

    public class EventorUserDbContext : IdentityDbContext<EventorUser>
    {
        public EventorUserDbContext()
            : base("UserDatabase")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("EventorUsers", "dbo"); // .Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<EventorUser>().ToTable("EventorUsers", "dbo");
        }
    }
}
