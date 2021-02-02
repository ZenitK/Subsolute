using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AsciiTreeDiagram;

namespace Subsolute
{
    public class SolutionBuilder
    {
        public async Task Build(ProjectNode root, string solutionName, string solutionPath)
        {
            var uniqueProjects = ExtractAllUniqueProjects(root);

            Console.WriteLine($"Total projects: {uniqueProjects.Count}");

            foreach (var project in uniqueProjects.OrderBy(p => p))
            {
                Console.WriteLine("\t-" + project);
            }

            var finalSolutionPath = ResolveSolutionPath(root, solutionPath);
            
            await CreateSolutionIfNotExists(solutionName, finalSolutionPath);

            var projectListArgument = string.Join(" ", uniqueProjects);

            await ExecuteDotnetProcess(finalSolutionPath, $"sln add {projectListArgument}");
        }

        private static string ResolveSolutionPath(ProjectNode root, string solutionPath) => 
            string.IsNullOrWhiteSpace(solutionPath) ? Path.GetDirectoryName(root.AbsolutePath) : solutionPath;

        private static async Task CreateSolutionIfNotExists(string solutionName, string solutionPath)
        {
            var fullSolutionPath = Path.Combine(solutionPath, $"{solutionName}.sln");
            
            if (!File.Exists(fullSolutionPath) || Directory.Exists(fullSolutionPath))
            {
                await CreateSolution(solutionName, solutionPath);
            }
            else
            {
                var foreground = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Solution at {solutionPath} already exists. Skipping.");
                Console.ForegroundColor = foreground;
            }
        }

        private static Task CreateSolution(string filename, string solutionPath)
        {
            var arguments = string.IsNullOrWhiteSpace(filename) ? "new sln" : $"new sln --name {filename}";

            return ExecuteDotnetProcess(solutionPath, arguments);
        }

        private static async Task<Process> ExecuteDotnetProcess(string solutionPath, string arguments)
        {
            const string command = "dotnet";
            var workingDirectory = solutionPath ?? ".";

            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = workingDirectory,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = command,
                Arguments = arguments
            };

            var process = new Process {StartInfo = startInfo};
            process.Start();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception(
                    $"received exit code {process.ExitCode} for command `{command} {arguments}` " +
                    $"in working directory `{workingDirectory}`");
            }
            
            return process;
        }

        /// <summary>
        /// Many projects like core libraries repeat multiple times, those we don't need to import multiple times 
        /// </summary>
        private static IReadOnlyList<string> ExtractAllUniqueProjects(ProjectNode root)
        {
            var flatTree = Flatten(root);

            return flatTree
                .Select(x => x.AbsolutePath)
                .Distinct()
                .ToList();
        }

        private static IEnumerable<ProjectNode> Flatten(ProjectNode projectNode)
        {
            var nodes = new List<ProjectNode> {projectNode};
            nodes.AddRange(projectNode.Children?.SelectMany(Flatten) ?? Array.Empty<ProjectNode>());
            return nodes;
        }
    }
}