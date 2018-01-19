# List Service Principals
Login and review all service principals in the selected Azure Active Directory

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fbretthackermsft%2Flist-service-principals%2Fmaster%2Fazuredeploy.json" target="_blank"><img src="http://azuredeploy.net/deploybutton.png"/></a>

__Details__
Calls the Microsoft Graph (beta) to retrieve all Service Principals in the tenant. (NOTE: beta APIs are subject to change - the data models may change before GA, and will require this app to be tweaked if so)

https://developer.microsoft.com/en-us/graph/docs/api-reference/beta/api/serviceprincipal_list

* ARM template deploys the following:
  * Azure Web App
* Requires the following (see step-by-step deployment instructions above for details):
  1. Azure AD application with the following:
    * Microsoft Graph - delegated permissions
      * Read directory data (to query for Service Principals)
    * Azure Active Directory - delegated permissions
      * Sign in and read your profile (default app permission)

The process for creating an Azure Active Directory application can be found at https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal#create-an-azure-active-directory-application


  ![alt text][App1]

[App1]: ./Images/Homepage.jpg "Home Page"