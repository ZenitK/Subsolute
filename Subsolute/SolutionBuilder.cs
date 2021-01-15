using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AsciiTreeDiagram;
using Xml2CSharp;
using static System.IO.Path;

namespace Subsolute
{
    public class SolutionBuilder
    {
        private readonly XmlSerializer _xmlSerializer = new(typeof(Project));

        private static string ExtractProjectGuid(Project deserializedProject, string projectPath)
        {
            var guids = deserializedProject
                .PropertyGroup
                .Select(x => x.ProjectGuid)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (guids.Count > 1)
            {
                throw new Exception($"The project {projectPath} seems to have multiple Guids " +
                                    $"[{string.Join(",", guids)}]");
            }

            return guids.SingleOrDefault();
        }

        private List<Node> ExtractChildren(Project deserializedProject, string parentFullPath) =>
            deserializedProject
                .ItemGroup
                .SelectMany(x => x.ProjectReference)
                .Select(x =>
                {
                    var fullPath = FindChildFullPath(parentFullPath, x.Include);
                    return BuildProjectTree(fullPath);
                })
                .ToList();

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

        private static void CheckIfFileExists(string projectPath)
        {
            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException($"csproj file {projectPath} not found");
            }
        }

        public Node BuildProjectTree(string projectPath)
        {
            CheckIfFileExists(projectPath);

            var projectName = GetFileName(projectPath);
            var deserializedProject = DeserializeProject(projectPath);

            var children = ExtractChildren(deserializedProject, parentFullPath: projectPath);
            var projectGuid = ExtractProjectGuid(deserializedProject, projectPath);

            return new Node
            {
                Name = projectName,
                AbsolutePath = projectPath,
                ProjectGuid = projectGuid,
                Children = children
            };
        }
    }
}