using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AzureFoundationToolkit.AzHelpers
{
    public class AzMgmtClient
    {
        private string clientDomain;
        private string clientId;
        private string clientSecret;

        public AzMgmtClient(string _clientDomain, string _clientId, string _clientSecret)
        {
            clientDomain = _clientDomain;
            clientId = _clientId;
            clientSecret = _clientSecret;

        }
        public string ClientRequest(string url)
        {
            var token = AzTokenTool.GetToken(clientDomain, clientId, clientSecret);
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        public List<LogicApp> GetLogicApps(string subscription, string azMgmtBaseUrl = "https://management.azure.com", string azMgmtApiVersion = "2016-06-01")
        {
            var logicApps = new List<LogicApp>();
            var url = $"{azMgmtBaseUrl}/subscriptions/{subscription}/providers/Microsoft.Logic/workflows?api-version={azMgmtApiVersion}";
            var next = false;

            do
            {
                var rawStringResponse = ClientRequest(url);
                var rawJsonResponse = JObject.Parse(rawStringResponse);

                logicApps.AddRange((from p in rawJsonResponse["value"]
                                    select new LogicApp
                                    {
                                        Name = (string)p["name"],
                                        Id = (string)p["id"],
                                        AccessEndPoint = (string)p["properties"]["accessEndpoint"],
                                        Created = (DateTime)p["properties"]["createdTime"],
                                        Changed = (DateTime)p["properties"]["changedTime"],
                                        State = (string)p["properties"]["state"],
                                        Kind = (string)(p["properties"]["definition"]["triggers"].SelectTokens("$..kind").Any() ? p["properties"]["definition"]["triggers"].SelectTokens("$..kind").First() : ""),
                                        Type = (string)(p["properties"]["definition"]["triggers"].SelectTokens("$..type").Any() ? p["properties"]["definition"]["triggers"].SelectTokens("$..type").First() : ""),
                                        Method = (string)(p["properties"]["definition"]["triggers"].SelectTokens("$...method").Any() ? p["properties"]["definition"]["triggers"].SelectTokens("$...method").First() : ""),
                                        Schema = (p["properties"]["definition"]["triggers"].SelectTokens("$...schema").Any() ? p["properties"]["definition"]["triggers"].SelectTokens("$...schema").First() : null)
                                    })
                            .ToList<LogicApp>());

                next = (rawJsonResponse["nextLink"] != null);
                if (next)
                {
                    url = (string)rawJsonResponse["nextLink"];
                }
            }
            while (next);

            return logicApps;

        }
    }
}
