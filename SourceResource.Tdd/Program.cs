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
            

            var className = "FilesResource";
            
            var csFileName = new DirectoryInfo("../../..").FullName + $"/{className}.cs";
            
            builder.CreateFile(csFileName, "Files",className);
            
            
            builder.ExtractIntoDirectory("Miles",Assembly.GetEntryAssembly());
        }
    }
}
