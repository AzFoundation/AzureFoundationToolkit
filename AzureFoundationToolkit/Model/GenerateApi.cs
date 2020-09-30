using System.Collections.Generic;
using CommandLine;

namespace AzureFoundationToolkit
{
    [Verb("apim-generate", HelpText = "Generate swagger for logic apps.")]
    public class GenerateApiOptions
    {
        [Option('h', "host", Required = true, HelpText = "Host name for api management")]
        public string Hostname { get; set; }

        [Option('x', "exclusions", Required = false, HelpText = "Strings to be excluded from the name of the operation.")]
        public IEnumerable<string> Exclusions { get; set; }

        [Option('s', "subscription", Required = true, HelpText = "Azure subscription to run against.")]
        public string Subscription { get; set; }

        [Option("client-domain", Required = true, HelpText = "Tenant's domain name to login against.")]
        public string ClientDomain { get; set; }

        [Option("client-id", Required = true, HelpText = "Client (application) id to authenticate.")]
        public string ClientId { get; set; }

        [Option("client-secret", Required = true, HelpText = "Client (Application) secret to authenticate")]
        public string ClientSecret { get; set; }

        [Option("api-title", Required = false, HelpText = "API title.")]
        public string ApiTitle { get; set; }

        [Option("api-version", Default = "1.0", Required = false, HelpText = "API version.")]
        public string ApiVersion { get; set; }

        [Option('v', "az-mgmt-apiversion", Default = "2016-06-01",  Required = false, HelpText = "Azure Management API version")]
        public string AzMgmtApiVersion { get; set; }

        [Option('u', "az-mgmt-url", Default = "https://management.azure.com", Required = false, HelpText = "Azure Management base url")]
        public string AzMgmtUrl { get; set; }

        [Option('o', "output", Required = false, HelpText = "Output file path, stores the swagger document into this file.")]
        public string OutputFilePath { get; set; }

        [Option('f', "filter", Default = false, Required = false, HelpText = "Enable filtering logic apps by name.")]
        public bool Filter { get; set; }

        [Option("name-starts-with", Default = "", Required = false, HelpText = "Filters logic apps by name starts with given string.")]
        public string NameStartsWith { get; set; }
    }
}
