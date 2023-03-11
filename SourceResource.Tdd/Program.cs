using System.IO;
using System.Reflection;
using Acidmanic.Utilities.SourceResource;

namespace SourceResource.Tdd
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new SourceDataBuilder();
            
            builder.ExtractIntoDirectory("Miles",Assembly.GetEntryAssembly());
        }
    }
}
