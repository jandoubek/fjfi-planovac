using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;

using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

using Eventor.Models;

namespace Eventor.App_Start
{
    public class EventorUserManager : UserManager<EventorUser>
    {
        public EventorUserManager(IUserStore<EventorUser> store)
            : base(store)
        {
        }

        public static EventorUserManager Create(IdentityFactoryOptions<EventorUserManager> options, IOwinContext context)
        {
            var manager = new EventorUserManager(new UserStore<EventorUser>(context.Get<EventorUserDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<EventorUser>(manager)
            {
                //AllowOnlyAlphanumericUserNames = false,
                //RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<EventorUser>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<EventorUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<EventorUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            MailMessage email = new MailMessage("xxx@hotmail.com", message.Destination);
            email.Subject = message.Subject;
            email.Body = message.Body;
            email.IsBodyHtml = true;
            var mailClient = new SmtpClient("smtp.live.com", 587) { Credentials = new NetworkCredential("xxx@hotmail.com", "password"), EnableSsl = true };
            return mailClient.SendMailAsync(email);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
