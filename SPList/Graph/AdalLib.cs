using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SPList.Graph
{
    public static class AdalLib
    {
        public static string GraphApiVersion = "beta";

        private static async Task<string> GetAccessToken()
        {
            //
            // Retrieve the user's name, tenantID, and access token since they are parameters used to query the Graph API.
            //
            string tenantId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            AuthenticationResult result = null;

            // Get the access token from the cache
            string userObjectID =
                ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")
                    .Value;
            var htxbase = new HttpContextWrapper(HttpContext.Current);
            AuthenticationContext authContext = new AuthenticationContext(Startup.Authority,
                new NaiveSessionCache(userObjectID, htxbase));
            if (authContext.TokenCache.Count == 0)
            {
                //htxbase.Response.Clear();
                htxbase.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = htxbase.Request.RawUrl },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
                return "";
            }

            ClientCredential credential = new ClientCredential(Startup.ClientId, Startup.AppKey);
            result = await authContext.AcquireTokenSilentAsync(Startup.GraphResourceId, credential,
                new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

            return result.AccessToken;
        }

        public static async Task<AdalResponse> GetResourceAsync(string request)
        {
            var res = new AdalResponse
            {
                Successful = true
            };

            var requestUrl = string.Format("{0}/{1}{2}", Startup.GraphResourceId, GraphApiVersion, request);

            string token = await GetAccessToken();
            if (token == "")
            {
                res.Successful = false;
                res.Message = "Reauthenticate";
                return res;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.SendAsync(req);
                    res.ResponseContent = await response.Content.ReadAsStringAsync();
                    res.StatusCode = response.StatusCode;
                    res.Message = response.ReasonPhrase;

                    if (!response.IsSuccessStatusCode)
                    {
                        res.Successful = false;
                        var serverError = JsonConvert.DeserializeObject<GraphError>(res.ResponseContent);
                        var reason = (response == null ? "N/A" : response.ReasonPhrase);
                        var serverErrorMessage = (serverError.Error == null) ? "N/A" : serverError.Error.Message;
                        res.Message = string.Format("(Server response: {0}. Server detail: {1})", reason, serverErrorMessage);
                        return res;
                    }

                    return res;
                }
                catch (Exception ex)
                {
                    res.Successful = false;
                    res.Message = ex.Message;
                    return res;
                }
            }
        }
    }
}