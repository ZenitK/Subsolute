using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsciiTreeDiagram;

namespace Subsolute
{
    public class SolutionBuilder
    {
        public void Build(ProjectNode root, string solutionName = "")
        {
            var uniqueProjects = ExtractAllUniqueProjects(root);

            Console.WriteLine($"Total projects: {uniqueProjects.Count}");

            foreach (var allProject in uniqueProjects.OrderBy(p => p.Name))
            {
                Console.WriteLine(allProject.Name);
            }

            var solutionGuid = Guid.NewGuid().ToString("B");

            var slnContentBuilder = new StringBuilder();

            slnContentBuilder.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00");
            slnContentBuilder.AppendLine();

            var projectDeclarations = uniqueProjects
                .Select(project => BuildProjectDeclaration(project, solutionGuid));

            slnContentBuilder.AppendLine(string.Join("\n", projectDeclarations));

            Console.WriteLine(slnContentBuilder.ToString());
        }

        private static string BuildProjectDeclaration(Project project, string solutionGuid)
        {
            var (name, absolutePath, guid) = project;

            var projectGuid = (string.IsNullOrWhiteSpace(guid)
                    ? Guid.NewGuid()
                    : Guid.Parse(guid))
                .ToString("B");

            var projectNameWithoutExtension = name.Substring(
                0,
                name.LastIndexOf('.'));

            var firstRow =
                $"Project(\"{solutionGuid}\") = \"{projectNameWithoutExtension}\", \"{absolutePath}\", \"{projectGuid}\"" +
                "\nEndProject";

            return firstRow;
        }

        /// <summary>
        /// Many projects like core libraries repeat multiple times, those we don't need to import multiple times 
        /// </summary>
        private static IReadOnlySet<Project> ExtractAllUniqueProjects(ProjectNode root)
        {
            var flatTree = Flatten(root);

            return flatTree
                .Select(x => new Project(x.Name, x.AbsolutePath, x.ProjectGuid))
                .ToHashSet();
        }

        private static IEnumerable<ProjectNode> Flatten(ProjectNode projectNode) =>
            projectNode
                .Children
                .SelectMany(Flatten)
                .Concat(new[] {projectNode});

        // Using a new data structure in order to have a proper hash value, the children's list is in the way otherwise
        private record Project(string Name, string AbsolutePath, string ProjectGuid);
    }
}