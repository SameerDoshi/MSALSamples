using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates; //Only import this if you are using certificate
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
namespace MSALSamples
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

           
            string email = req.Query["email"];

            
            string responseMessage = string.IsNullOrEmpty(email)
                ? "Pass an email the query string"
                : $"Hello, {email} executed successfully.";
            string clientSecret = Environment.GetEnvironmentVariable("ClientSecret");
            string clientId = Environment.GetEnvironmentVariable("ClientId");
            string authority = Environment.GetEnvironmentVariable("Authority");
            string apiUrl = Environment.GetEnvironmentVariable("ApiUrl");
            string graphQueryURI = $"{apiUrl}v1.0/users?$filter=startswith(userPrincipalName%2C+'{email}')";

            IConfidentialClientApplication app;
            app = ConfidentialClientApplicationBuilder.Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(new Uri(authority))
                    .Build();
            string[] scopes = new string[] { $"{apiUrl}.default" };

            AuthenticationResult result = null;
            try
            {

                result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();
                log.LogInformation("Token acquired");
                
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                log.LogError("Scope provided is not supported");    
            }

            if (result != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    
                    var defaultRequetHeaders = client.DefaultRequestHeaders;
                    defaultRequetHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    defaultRequetHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
                    
                    client.BaseAddress = new Uri("https://www.google.com");
                    
                    HttpResponseMessage resa = await client.GetAsync(graphQueryURI);
                    string json = await resa.Content.ReadAsStringAsync();
                    log.LogInformation(json);
                    JObject o = JsonConvert.DeserializeObject(json) as JObject;
                    responseMessage = (string)o.SelectToken("value[0].id");
                }
            }
            return new OkObjectResult(responseMessage);
        }

       
    }


}
