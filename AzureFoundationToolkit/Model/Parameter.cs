using System;
using System.Collections.Generic;

namespace AzureFoundationToolkit
{
    public class Parameter
    {
        public string name { get; set; }
        public string @in { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
        public string @default { get; set; }
        public List<String> @enum { get; set; }
    }
}
