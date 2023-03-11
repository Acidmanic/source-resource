using System;
using System.IO;
using Acidmanic.Utilities.NamingConventions;
using Acidmanic.Utilities.SourceResource;
using Acidmanic.Utilities.SourceResourceTool.Commands.Arguments;
using CoreCommandLine;
using CoreCommandLine.Attributes;
using Microsoft.Extensions.Logging;

namespace Acidmanic.Utilities.SourceResourceTool.Commands
{
    [Subcommands(typeof(ClassName),typeof(SourceDirectory))]
    [CommandName("pack","p")]
    public class Pack:CommandBase
    {
        public override bool Execute(Context context, string[] args)
        {
            if (AmIPresent(args))
            {
                var sourceDirectory = context.ReadArgument<SourceDirectory>();
                var className = context.ReadArgument<ClassName>();

                if (!className || !sourceDirectory)
                {
                    Logger.LogError("You have to provide all mandatory arguments. run pack --help for details.");

                    return true;
                }

                var builder = new SourceDataBuilder();

                var pascalClassName = new NamingConvention()
                    .Convert(className.Value, ConventionDescriptor.Standard.Pascal);

                var csFileName = AtCurrentDirectory(pascalClassName + ".cs");
                
                builder.CreateFile( csFileName,sourceDirectory.Value,pascalClassName);

                return true;
            }
            return false;
        }

        public override string Description =>
            "Reads the given directory (source-directory) and packages it's content " +
            "into a c# class code. The created class would be written in the " +
            "directory which application is being executed in.";
        
        private string AtCurrentDirectory(string fileName)
        {
            var location = new DirectoryInfo(".").FullName;

            location ??= ".";

            return Path.Combine(location, fileName);
        }
    }
}