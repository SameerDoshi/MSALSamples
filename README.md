# MSALSamples
Microsoft Authentication Library Samples

Cretes a function that uses MSAL to Auth against Azure AD and return user id from email address
To setup:
   1. Create an Azure AD App Registration as outlined [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-netcore-daemon)
   2. Run locally by populating localsettings.json
   3. Deploy to Azure 
   4. Add appsettings similar to the ones in localsettings to your function app
   5. Please ensure your app is protected by FunctionKey or HostKey

This is based on: [https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-netcore-daemon](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-netcore-daemon)

The tutorial/sample code is provided as is and is not meant for use on a production environment. It is provided only for illustrative purposes. The end user must test and modify the sample to suit their target environment.  Microsoft can make no representation concerning the content of this sample. Microsoft is providing this information only as a convenience to you. This is to inform you that Microsoft has not tested the sample and therefore cannot make any representations regarding the quality, safety, or suitability of any code or information found here. 