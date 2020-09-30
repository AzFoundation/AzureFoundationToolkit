using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AzureFoundationToolkit.AzHelpers;
using Newtonsoft.Json;

namespace AzureFoundationToolkit
{
    public class ApimGenerate
    {
        private readonly GenerateApiOptions opt;

        public ApimGenerate(GenerateApiOptions _opt)
        {
            opt = _opt;
        }

        internal static void RunOptions(GenerateApiOptions obj)
        {
            var tool = new ApimGenerate(obj);
            tool.Execute();
        }

        protected void Execute()
        {
            var dict = new Dictionary<string, bool>();
            var client = new AzMgmtClient(opt.ClientDomain, opt.ClientId, opt.ClientSecret);
            var logicApps = client.GetLogicApps(opt.Subscription, opt.AzMgmtUrl, opt.AzMgmtApiVersion);
            var api = $@"{{""swagger"":""2.0"",""info"":{{""title"":""{opt.ApiTitle}"",""version"":""1.0""}},""host"":""{opt.Hostname}"",""schemes"":[""https""],""paths"":{{";


            IEnumerable<LogicApp> filteredLAs;
            if (opt.Filter)
            {
                filteredLAs = logicApps.Where(i => i.Name.StartsWith(opt.NameStartsWith));
            }
            else
            {
                filteredLAs = logicApps;
            }

            foreach (var logicApp in filteredLAs)
            {
                string apiName = string.Join("/", logicApp.Name.Split("-").Where(j => !opt.Exclusions.Contains(j)).Select(j => j)).ToLower();
                if (!dict.ContainsKey(apiName))
                {
                    dict.Add(apiName, true);
                    api += $@" ""/{apiName}"": ";
                    string[] ids = logicApp.Id.Split('/');
                    string subscription = ids[2];
                    string resourceGroup = ids[4];
                    string resource = ids[8];
                    var operation = new
                    {
                        post = new
                        {
                            description = $"Subscription : {subscription} | Resource Group : {resourceGroup} | Resource : {resource} ",
                            operationId = logicApp.Name,
                            summary = "/" + apiName,
                            tags = new string[] { apiName.Split('/').Count() > 1 ? apiName.Split('/')[0] : "Irregular", "State:" + logicApp.State },
                            responses = new
                            {
                                _200 = new
                                {
                                    description = "Successful result"
                                }
                            },
                            parameters = new List<Parameter>{
                    new Parameter {
                        name = "Subscription",
                        @in = "header",
                        required = true,
                        type = "string",
                        @default = subscription,
                        @enum = new List<string>{ subscription }
                    },
                    new Parameter {
                        name = "ResourceGroup",
                        @in = "header",
                        required = true,
                        type = "string",
                        @default = resourceGroup,
                        @enum = new List<string>{ resourceGroup }
                    },
                    new Parameter {
                        name = "Resource",
                        @in = "header",
                        required = true,
                        type = "string",
                        @default = resource,
                        @enum = new List<string>{ resource }
                    }
                }
                        }
                    };
                    api += JsonConvert.SerializeObject(operation) + ",";

                }
            }
            api = api.Remove(api.Length - 1, 1);
            api += "},\"tags\":[]}";
            api = api.Replace("_200", "200");
            File.WriteAllText(opt.OutputFilePath, api);
        }
    }
}
