using System;
using Acidmanic.Utilities.SourceResource;
namespace Resources
{
    public class FilesResource:ISourceData
    {
        public string Name => "FilesResource";
        
        public string ReadAll()
        {
            return  "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";
        }
    }
}