using System;
using Newtonsoft.Json.Linq;

namespace AzureFoundationToolkit
{
    public class LogicApp
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string State { get; set; }
        public string AccessEndPoint { get; set; }
        public DateTime Created { get; set; }
        public DateTime Changed { get; set; }
        public string Type { get; set; }
        public string Kind { get; set; }
        public string Method { get; set; }
        public JToken Schema { get; set; }
    }

}
