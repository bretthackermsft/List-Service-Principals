using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SPList.Graph;
using Microsoft.Owin.Security.Notifications;
using Utils;

namespace SPList
{
    public partial class Startup
    {
        public static string ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string Authority = ConfigurationManager.AppSettings["ida:AADInstance"] + "common";
        public static string AppKey = ConfigurationManager.AppSettings["ida:AppKey"];
        public static string GraphResourceId = ConfigurationManager.AppSettings["ida:GraphResourceId"];

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions { });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = ClientId,
                    Authority = Authority,
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                    {
                        // instead of using the default validation (validating against a single issuer value, as we do in line of business apps), 
                        // we inject our own multitenant validation logic
                        ValidateIssuer = false,
                        // If the app needs access to the entire organization, then add the logic
                        // of validating the Issuer here.
                        // IssuerValidator
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {   
                        SecurityTokenValidated = (context) =>
                        {
                            // If your authentication logic is based on users then add your logic here
                            return Task.FromResult(0);
                        } ,                    
                        AuthenticationFailed = (context) =>
                        {
                            // Pass in the context back to the app
                            context.OwinContext.Response.Redirect("/Home/Error");
                            context.HandleResponse(); // Suppress the exception
                            return Task.FromResult(0);
                        },
                        AuthorizationCodeReceived = OnAuthorizationCodeReceived
                    }
                });
        }

        /// <summary>
        /// Modified call to naivesession per 
        /// https://stackoverflow.com/questions/43908344/httpcontext-current-is-null-on-tokencache-beforeaccess
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedNotification context)
        {
            try
            {
                var code = context.Code;

                ClientCredential credential = new ClientCredential(ClientId, AppKey);
                string userObjectID = context.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                AuthenticationContext authContext = new AuthenticationContext(Authority, new NaiveSessionCache(userObjectID, context.OwinContext.Environment["System.Web.HttpContextBase"] as HttpContextBase));

                // If you create the redirectUri this way, it will contain a trailing slash.  
                // Make sure you've registered the same exact Uri in the Azure Portal (including the slash).
                Uri uri = new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path));

                AuthenticationResult result = await authContext.AcquireTokenByAuthorizationCodeAsync(code, uri, credential, GraphResourceId);
            }
            catch (Exception ex)
            {
                Logging.WriteToAppLog("Error caching auth code", System.Diagnostics.EventLogEntryType.Error, ex);
                throw;
            }
        }
    }
}
