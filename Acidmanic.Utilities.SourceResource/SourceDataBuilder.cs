using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Acidmanic.Utilities.Reflection;
using Acidmanic.Utilities.Reflection.Extensions;


namespace Acidmanic.Utilities.SourceResource
{
    public class SourceDataBuilder
    {
        public byte[] RetrieveData(ISourceData sourceData)
        {
            var base64 = sourceData.ReadAll();

            var data = Convert.FromBase64String(base64);

            return data;
        }

        public void ExtractIntoDirectory(string targetDirectory, params Assembly[] assemblies)
        {
            var sources = EnumerateSourceData(assemblies);

            sources.ForEach(s => ExtractIntoDirectory(s, targetDirectory));
        }
        
        public void ExtractIntoDirectory(string targetDirectory,string className, params Assembly[] assemblies)
        {
            var foundSource = EnumerateSourceData(assemblies)
                .FirstOrDefault(s => string.Equals(s.Name, className,
                    StringComparison.CurrentCultureIgnoreCase));

            if (foundSource != null)
            {
                ExtractIntoDirectory(foundSource,targetDirectory);
            }
        }

        private List<ISourceData> EnumerateSourceData(Assembly[] assemblies)
        {
            var sourceDataList = new List<ISourceData>();

            var instantiate = new ObjectInstantiator();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetAvailableTypes()
                    .Where(TypeCheck.Implements<ISourceData>)
                    .Select(t => instantiate.BlindInstantiate(t) as ISourceData)
                    .Where(o => o != null);

                sourceDataList.AddRange(types);
            }

            return sourceDataList;
        }

        public void ExtractIntoDirectory(ISourceData sourceData, string targetDirectory)
        {
            var zipBytes = RetrieveData(sourceData);

            var tempFile = AtCurrentDirectory(Guid.NewGuid().ToString());

            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }

            File.WriteAllBytes(tempFile, zipBytes);

            if (Directory.Exists(targetDirectory))
            {
                Directory.Delete(targetDirectory, true);
            }

            ZipFile.ExtractToDirectory(tempFile, targetDirectory);

            File.Delete(tempFile);
        }

        public void CreateFile(string path, byte[] data, string name, string @namespace = "Resources")
        {
            var content = CreateFileContent(data, name, @namespace);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, content);
        }

        private string AtCurrentDirectory(string fileName)
        {
            var location = Assembly.GetEntryAssembly()?.Location;

            if (location != null)
            {
                location = new FileInfo(location).Directory?.FullName;
            }

            location ??= ".";

            return Path.Combine(location, fileName);
        }

        public void CreateFile(string path, string sourceDirectory, string name, string @namespace = "Resources")
        {
            var content = CreateFileContent(sourceDirectory, name, @namespace);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, content);
        }

        public string CreateFileContent(string sourceDirectory, string name, string @namespace = "Resources")
        {
            var tempFile = AtCurrentDirectory(Guid.NewGuid().ToString());

            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }

            ZipFile.CreateFromDirectory(sourceDirectory, tempFile);

            var data = File.ReadAllBytes(tempFile);

            File.Delete(tempFile);

            var content = CreateFileContent(data, name, @namespace);

            return content;
        }

        public string CreateFileContent(byte[] data, string name, string @namespace = "Resources")
        {
            var content = _template;

            content = content.Replace("__NAME__", $"\"{name}\"");
            content = content.Replace("__NAME_SPACE__", $"{@namespace}");
            content = content.Replace("__CLASS_NAME__", $"{name}");

            var base64 = Convert.ToBase64String(data)
                .Replace(" ", "")
                .Replace("\t", "")
                .Replace("\r", "")
                .Replace("\n", "");

            content = content.Replace("__CONTENT__", $"\"{base64}\"");

            return content;
        }

        private readonly string _template = @"
using System;
using Acidmanic.Utilities.SourceResource;
namespace __NAME_SPACE__
{
    public class __CLASS_NAME__:ISourceData
    {
        public string Name => __NAME__;
        
        public string ReadAll()
        {
            return  __CONTENT__;
        }
    }
}
".Trim();
    }
}