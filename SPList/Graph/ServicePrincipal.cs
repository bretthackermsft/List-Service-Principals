using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Graph;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SPList.Graph
{
    public class ServicePrincipal
    {
        [DisplayName("Account Enabled?")]
        [JsonProperty(PropertyName = "accountEnabled")]
        public bool AccountEnabled { get; set; }

        //[JsonProperty(PropertyName = "addIns")]
        //[DisplayName("Add-Ins")]
        //public IEnumerable<SPAddIn> AddIns { get; set; }

        [DisplayName("App Display Name")]
        [JsonProperty(PropertyName = "appDisplayName")]
        public string AppDisplayName { get; set; }

        [DisplayName("App ID")]
        [JsonProperty(PropertyName = "appId")]
        public string AppId { get; set; }

        [DisplayName("App Owner Org ID")]
        [JsonProperty(PropertyName = "appOwnerOrganizationId")]
        public string AppOwnerOrganizationId { get; set; }

        [DisplayName("App Role Assignment Required?")]
        [JsonProperty(PropertyName = "appRoleAssignmentRequired")]
        public bool AppRoleAssignmentRequired { get; set; }

        [DisplayName("App Roles")]
        [JsonProperty(PropertyName = "appRoles")]
        public IEnumerable<SPAppRole> AppRoles { get; set; }

        [DisplayName("Display Name")]
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [DisplayName("Error URL")]
        [JsonProperty(PropertyName = "errorUrl")]
        public string ErrorUrl { get; set; }

        [DisplayName("Home Page")]
        [JsonProperty(PropertyName = "homepage")]
        public string Homepage { get; set; }

        [DisplayName("ID")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [DisplayName("Key Credentials")]
        [JsonProperty(PropertyName = "keyCredentials")]
        public IEnumerable<SPKeyCredential> KeyCredentials { get; set; }

        [DisplayName("Logout URL")]
        [JsonProperty(PropertyName = "logoutUrl")]
        public string LogoutUrl { get; set; }

        [DisplayName("Published Permission Scopes")]
        [JsonProperty(PropertyName = "publishedPermissionScopes")]
        public IEnumerable<SPPermissions> PublishedPermissionScopes { get; set; }

        [DisplayName("Password Credentials")]
        [JsonProperty(PropertyName = "passwordCredentials")]
        public IEnumerable<SPCredential> PasswordCredentials { get; set; }

        [DisplayName("Preferred Token Signing Key Thumbprint")]
        [JsonProperty(PropertyName = "preferredTokenSigningKeyThumbprint")]
        public string PreferredTokenSigningKeyThumbprint { get; set; }

        [DisplayName("Publisher Name")]
        [JsonProperty(PropertyName = "publisherName")]
        public string PublisherName { get; set; }

        [DisplayName("Reply URLs")]
        [JsonProperty(PropertyName = "replyUrls")]
        public IEnumerable<string> ReplyUrls { get; set; }

        [DisplayName("SAML Metadata URL")]
        [JsonProperty(PropertyName = "samlMetadataUrl")]
        public string SamlMetadataUrl { get; set; }

        [DisplayName("Service Principal Names")]
        [JsonProperty(PropertyName = "servicePrincipalNames")]
        public IEnumerable<string> ServicePrincipalNames { get; set; }

        [DisplayName("Tags")]
        [JsonProperty(PropertyName = "tags")]
        public IEnumerable<string> Tags { get; set; }

        public static async Task<IEnumerable<ServicePrincipal>> GetServicePrincipalsAsync()
        {
            var res = new List<ServicePrincipal>();

            AdalResponse serverResponse = null;
            var rolesUri = "/servicePrincipals";
            serverResponse = await AdalLib.GetResourceAsync(rolesUri);

            if (serverResponse.Successful)
            {
                JObject data = JObject.Parse(serverResponse.ResponseContent);
                IList<JToken> roles = data["value"].ToList();
                foreach (var role in roles)
                {
                    var item = JsonConvert.DeserializeObject<ServicePrincipal>(role.ToString());
                    res.Add(item);
                }
            }

            return res;
        }
        public static async Task<IEnumerable<SPoAuth2Perm>> GetOAuth2Permissions(string spid)
        {

            var res = new List<SPoAuth2Perm>();

            AdalResponse serverResponse = null;
            var permsUri = string.Format("/serviceprincipals/{0}/oAuth2Permissiongrants", spid);
            serverResponse = await AdalLib.GetResourceAsync(permsUri);

            if (serverResponse.Successful)
            {
                JObject data = JObject.Parse(serverResponse.ResponseContent);
                IList<JToken> perms = data["value"].ToList();
                foreach (var perm in perms)
                {
                    var item = JsonConvert.DeserializeObject<SPoAuth2Perm>(perm.ToString());
                    res.Add(item);
                }
            }

            return res;
        }
    }

    public class SPAppRole
    {
        [DisplayName("Allowed Member Types")]
        [JsonProperty(PropertyName = "allowedMemberTypes")]
        public IEnumerable<string> AllowedMemberTypes { get; set; }

        [DisplayName("Description")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DisplayName("Display Name")]
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [DisplayName("ID")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [DisplayName("Is Enabled?")]
        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [DisplayName("Origin")]
        [JsonProperty(PropertyName = "origin")]
        public string Origin { get; set; }

        [DisplayName("Value")]
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class SPKeyCredential
    {
        [DisplayName("Custom Key Identifier")]
        [JsonProperty(PropertyName = "customKeyIdentifier")]
        public byte[] CustomKeyIdentifier { get; set; }

        [DisplayName("End DateTime")]
        [JsonProperty(PropertyName = "endDateTime")]
        public DateTime EndDateTime { get; set; }

        [DisplayName("KeyID")]
        [JsonProperty(PropertyName = "keyId")]
        public string KeyId { get; set; }

        [DisplayName("Start DateTime")]
        [JsonProperty(PropertyName = "startDateTime")]
        public DateTime StartDateTime { get; set; }

        [DisplayName("Type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [DisplayName("Usage")]
        [JsonProperty(PropertyName = "usage")]
        public string Usage { get; set; }

        [DisplayName("Key")]
        [JsonProperty(PropertyName = "key")]
        public byte[] Key { get; set; }
    }

    //public class SPAddIn
    //{

    //}

    public class SPCredential
    {
        [DisplayName("Custom Key Identifier")]
        [JsonProperty(PropertyName = "customKeyIdentifier")]
        public byte[] CustomKeyIdentifier { get; set; }

        [DisplayName("End Date")]
        [JsonProperty(PropertyName = "endDate")]
        public DateTime EndDate { get; set; }

        [DisplayName("Key ID")]
        [JsonProperty(PropertyName = "keyId")]
        public string KeyId { get; set; }

        [DisplayName("Start Date")]
        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate { get; set; }

        [DisplayName("Value")]
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class SPPermissions
    {
        [DisplayName("Admin Consent Description")]
        [JsonProperty(PropertyName = "adminConsentDescription")]
        public string AdminConsentDescription { get; set; }

        [DisplayName("Admin Consent Display Name")]
        [JsonProperty(PropertyName = "adminConsentDisplayName")]
        public string AdminConsentDisplayName { get; set; }

        [DisplayName("ID")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [DisplayName("Is Enabled?")]
        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [DisplayName("Origin")]
        [JsonProperty(PropertyName = "origin")]
        public string Origin { get; set; }

        [DisplayName("Type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [DisplayName("User Consent Description")]
        [JsonProperty(PropertyName = "userConsentDescription")]
        public string UserConsentDescription { get; set; }

        [DisplayName("User Consent Display Name")]
        [JsonProperty(PropertyName = "userConsentDisplayName")]
        public string UserConsentDisplayName { get; set; }

        [DisplayName("Value")]
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class SPoAuth2Perm
    {
        [DisplayName("Client Id")]
        [JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }

        [DisplayName("Consent Type")]
        [JsonProperty(PropertyName = "consentType")]
        public string ConsentType { get; set; }

        [DisplayName("Expiry Time")]
        [JsonProperty(PropertyName = "expiryTime")]
        public DateTime ExpiryTime { get; set; }

        [DisplayName("ID")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [DisplayName("Principal ID")]
        [JsonProperty(PropertyName = "principalId")]
        public string PrincipalId { get; set; }

        [DisplayName("Resource ID")]
        [JsonProperty(PropertyName = "resourceId")]
        public string ResourceId { get; set; }

        [DisplayName("Scope")]
        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        [DisplayName("Start Time")]
        [JsonProperty(PropertyName = "startTime")]
        public DateTime StartTime { get; set; }
    }
}