using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AsciiTreeDiagram;
using Xml2CSharp;
using static System.IO.Path;

namespace Subsolute
{
    public class TreeBuilder
    {
        private readonly XmlSerializer _xmlSerializer = new(typeof(Project));

        public IEnumerable<ProjectNode> BuildProjectTree(params string[] projectPaths)
        {
            foreach (var projectPath in projectPaths)
            {
                CheckIfFileExists(projectPath);

                var projectName = GetFileName(projectPath);
                var deserializedProject = DeserializeProject(projectPath);

                var children = ExtractChildren(deserializedProject, parentFullPath: projectPath);

                yield return new ProjectNode(
                    Name: projectName,
                    AbsolutePath: projectPath,
                    Children: children);
            }
        }

        private List<ProjectNode> ExtractChildren(Project deserializedProject, string parentFullPath) =>
            deserializedProject
                .ItemGroup
                .SelectMany(x => x.ProjectReference)
                .Select(x =>
                {
                    var fullPath = FindChildFullPath(parentFullPath, x.Include);
                    return BuildProjectTree(fullPath);
                })
                .SelectMany(x => x)
                .ToList();

        private static void CheckIfFileExists(string projectPath)
        {
            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException($"csproj file {projectPath} not found");
            }
        }

        private static string FindChildFullPath(string parentFullPath, string childPath)
        {
            var includedProject = childPath.Replace('\\', '/');
            var parentDirectory = GetDirectoryName(parentFullPath);
            var projectPath = Combine(parentDirectory ?? throw new InvalidOperationException(), includedProject);
            var fullPath = GetFullPath(projectPath);
            return fullPath;
        }

        private Project DeserializeProject(string projectPath)
        {
            using var fileReader = new FileStream(projectPath, FileMode.Open);
            var deserializedProject = _xmlSerializer.Deserialize(fileReader) as Project;

            return deserializedProject;
        }
    }
}