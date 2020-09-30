using System;
using System.Collections.Generic;
using CommandLine;

namespace AzureFoundationToolkit
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<GenerateApiOptions>(args)
              .WithParsed(ApimGenerate.RunOptions)
              .WithNotParsed(HandleParseError);
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach(var error in errs)
            {
            }
            //handle errors
        }
    }
}
