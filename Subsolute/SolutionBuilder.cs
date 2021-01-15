using System.IO;
using System.Xml.Serialization;
using AsciiTreeDiagram;
using Xml2CSharp;

namespace Subsolute
{
    public class SolutionBuilder
    {
        private string ExtractProjectName(string fullProjectPath) => Path.GetFileName(fullProjectPath);
        
        public Node BuildProjectTree(string fullProjectPath)
        {
            if (!File.Exists(fullProjectPath))
            {
                throw new FileNotFoundException($"csproj file {fullProjectPath} not found");
            }

            var xmlSerializer = new XmlSerializer(typeof(Project));
            using var fileReader = new FileStream(fullProjectPath, FileMode.Open);

            var deserializedProject = (Project) xmlSerializer.Deserialize(fileReader);

            var projectName = ExtractProjectName(fullProjectPath);
            
            return new Node {Name = projectName};
        }
        
        // public static async Task<string> BuildSolution(string fullProjectPath)
        // {
        //     if (!File.Exists(fullProjectPath))
        //     {
        //         throw new FileNotFoundException($"csproj file {fullProjectPath} not found");
        //     }
        //
        //     using var fileStream = File.OpenText(fullProjectPath);
        //     using var reader = XmlReader.Create(fileStream);
        //     
        //     throw new NotImplementedException();
        // }
    }
}