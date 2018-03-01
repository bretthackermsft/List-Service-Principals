# List Service Principals
Login and review all service principals in the selected Azure Active Directory

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fbretthackermsft%2Flist-service-principals%2Fmaster%2Fazuredeploy.json" target="_blank"><img src="http://azuredeploy.net/deploybutton.png"/></a>


  ![alt text][App1]

__Details__

Calls the Microsoft Graph (beta) to retrieve all Service Principals in the tenant. (NOTE: beta APIs are subject to change - the data models may change before GA, and will require this app to be tweaked if so)

https://developer.microsoft.com/en-us/graph/docs/api-reference/beta/api/serviceprincipal_list

* ARM template deploys the following:
  * Azure Web App
* Requires the following:
  1. Azure AD application with the following:
    * Microsoft Graph - delegated permissions
      * Read directory data (to query for Service Principals)
    * Azure Active Directory - delegated permissions
      * Sign in and read your profile (default app permission)

The process for creating an Azure Active Directory application can be found at https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal#create-an-azure-active-directory-application

## As-Is Code

This code is made available as a sample to demonstrate usage of the Microsoft Graph API to visualize some aspects of all deployed service principals. It should be customized by your dev team or a partner, and should be reviewed before being deployed in a production scenario.

## Contributing

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.


[App1]: ./Images/Homepage.jpg "Home Page"