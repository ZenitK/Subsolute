using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Subsolute
{
    public class SolutionBuilder 
    {
        public static async Task<string> Build(string fullProjectPath)
        {
            if (!File.Exists(fullProjectPath))
            {
                throw new FileNotFoundException($"csproj file {fullProjectPath} not found");
            }

            using var fileStream = File.OpenText(fullProjectPath);
            using var reader = XmlReader.Create(fileStream);
            
            throw new NotImplementedException();
        }
    }
}