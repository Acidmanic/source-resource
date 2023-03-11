using System;

namespace Acidmanic.Utilities.SourceResource
{
    public class ExampleSourceData:ISourceData
    {
        public string Name => "{{NAME}}";
        
        public string ReadAll()
        {
            return  "__CONTENT__";
        }
    }
}