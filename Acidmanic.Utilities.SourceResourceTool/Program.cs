using System;
using CoreCommandLine.DotnetDi;
using Microsoft.Extensions.Logging.LightWeight;

namespace Acidmanic.Utilities.SourceResourceTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger().Shorten();

            var app = new DotnetCommandlineApplicationBuilder<SrtApplication>()
                .Title("Source-Resource Application")
                .Description("Helps packaging resources when content resources are not allowed.")
                .UseLogger(logger)
                .UseStartup<Startup>()
                .Build();
            
            app.Execute(args);
        }
    }
}
