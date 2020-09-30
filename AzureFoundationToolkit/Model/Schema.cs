using System.Collections.Generic;

namespace AzureFoundationToolkit
{
    public class Schema
    {
        public string type { get; set; }
        public string @default { get; set; }
        public List<string> @enum { get; set; }
    }
}
