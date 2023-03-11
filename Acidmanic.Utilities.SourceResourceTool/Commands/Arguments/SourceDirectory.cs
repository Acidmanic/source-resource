using Acidmanic.Utilities.Results;
using CoreCommandLine;
using CoreCommandLine.Attributes;

namespace Acidmanic.Utilities.SourceResourceTool.Commands.Arguments
{
    [CommandName("source-directory", "sd")]
    public class SourceDirectory : CommandBase
    {
        public override bool Execute(Context context, string[] args)
        {
            if (AmIPresent(args))
            {

                var value = ReadMyValue(args);
                
                context.Set(nameof(SourceDirectory), value);

                return true;
            }

            return false;
        }

        public override string Description =>
            "The address of the directory which is supposed to be stored in a class code.";

        public Result<string> ReadMyValue(string[] args)
        {
            var index = IndexOf(NameBundle, args);

            if (index > 0 && index < args.Length - 1)
            {
                return new Result<string>(true, args[index + 1]);
            }

            return new Result<string>().FailAndDefaultValue();
        }
    }
}