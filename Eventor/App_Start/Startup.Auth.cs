using Eventor.App_Start;
using Eventor.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Owin;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eventor
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(EventorDbContext.GetInstance);
            app.CreatePerOwinContext<EventorUserManager>(EventorUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<EventorUserManager, EventorUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            var facebookAuthenticationOptions = new FacebookAuthenticationOptions()
            {
                AppId = "720122004735504",
                AppSecret = "e8d12d3d814e7d2092b089e35099115c",
            };

            facebookAuthenticationOptions.Scope.Add("email");
            facebookAuthenticationOptions.Provider = new FacebookAuthenticationProvider()
            {
                OnAuthenticated = (context) =>
                {
                    context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                    foreach (var claim in context.User)
                    {
                        var claimType = string.Format("urn:facebook:{0}", claim.Key);
                        var claimValue = claim.Value.ToString();

                        switch (claimType)
                        {
                            case "urn:facebook:first_name":
                                context.Identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.GivenName, claimValue));
                                break;
                            case "urn:facebook:last_name":
                                context.Identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Surname, claimValue));
                                break;
                            default:
                                break;
                        }
                       
                        if (!context.Identity.HasClaim(claimType, claimValue))
                            context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, "XmlSchemaString", "Facebook"));
                    }

                    return Task.FromResult(0);
                }
            };
            app.UseFacebookAuthentication(facebookAuthenticationOptions);

            app.UseGoogleAuthentication(
                clientId: "291693930204-3rom85lbpnojke0ug0ssp71n3kqa0cig.apps.googleusercontent.com",
                clientSecret: "A-uSvcBlKzYj6FeWAWKNJREh");
        }
    }
}