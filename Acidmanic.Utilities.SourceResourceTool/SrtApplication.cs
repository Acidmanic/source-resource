using Acidmanic.Utilities.SourceResourceTool.Commands;
using CoreCommandLine;
using CoreCommandLine.Attributes;

namespace Acidmanic.Utilities.SourceResourceTool
{
    [Subcommands(typeof(Pack))]
    public class SrtApplication:CommandLineApplication
    {
        
    }
}