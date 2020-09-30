using RestSharp;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureFoundationToolkit.AzHelpers
{
    public class AzTokenTool
    {
        public static string GetToken(string clientDomain, string clientId, string clientSecret, string azMgmtUrl = "https://management.azure.com")
        {
            var client = new RestClient($"https://login.microsoftonline.com/{clientDomain}/oauth2/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", $"{clientId}");
            request.AddParameter("client_secret", $"{clientSecret}");
            request.AddParameter("resource", $"{azMgmtUrl}");
            var response = client.Execute(request);
            var tokenObject = JObject.Parse(response.Content);
            return (string)tokenObject["access_token"];
        }
    }
}
