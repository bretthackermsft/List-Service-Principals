# List Service Principals
Login and review all service principals in the selected Azure Active Directory

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fbretthackermsft%2Flist-service-principals%2Fmaster%2Fazuredeploy.json" target="_blank"><img src="http://azuredeploy.net/deploybutton.png"/></a>

__Details__
Calls the Microsoft Graph (beta) to retrieve all Service Principals in the tenant. 

https://developer.microsoft.com/en-us/graph/docs/api-reference/beta/api/serviceprincipal_list

* ARM template deploys the following:
  * Azure Web App
* Requires the following (see step-by-step deployment instructions above for details):
  1. Azure AD application with the following:
    * Microsoft Graph - delegated permissions
      * Read directory data

  ![alt text][App1]

[App1]: ./Images/Homepage.jpg "Home Page"