namespace Acidmanic.Utilities.SourceResource
{
    public interface ISourceData
    {
        
        string Name { get; }

        string ReadAll();

    }
}