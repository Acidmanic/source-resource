using Acidmanic.Utilities.Results;
using CoreCommandLine;

namespace Acidmanic.Utilities.SourceResourceTool
{
    public static  class ContextExtensions
    {


        public static Result<string> ReadArgument<TArgumentCommand>(this Context context)
        {
            var key = typeof(TArgumentCommand).Name;

            return context.Get(key, new Result<string>().FailAndDefaultValue());
        }
    }
}