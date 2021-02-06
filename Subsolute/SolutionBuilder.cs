using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Subsolute
{
    public class SolutionBuilder
    {
        public Task Build(ProjectNode root, string solutionName, string solutionPath) =>
            Build(new[] {root}, solutionName, solutionPath);
        
        public async Task Build(IEnumerable<ProjectNode> roots, string solutionName, string solutionPath)
        {
            var uniqueProjects = roots.SelectMany(ExtractAllUniqueProjects).Distinct().ToList();

            PrintStatus(uniqueProjects);

            var finalSolutionPath = ResolveSolutionPath(solutionPath);
            
            await CreateSolutionIfNotExists(solutionName, finalSolutionPath);

            var projectListArgument = string.Join(" ", uniqueProjects);

            await AddProjectsToSolution(solutionName, finalSolutionPath, projectListArgument);
        }

        private static Task AddProjectsToSolution(
            string solutionName, 
            string finalSolutionPath,
            string projectListArgument) =>
            ExecuteDotnetProcess(
                finalSolutionPath,
                $"sln {solutionName}.sln add {projectListArgument}");

        private static void PrintStatus(List<string> uniqueProjects)
        {
            Console.WriteLine($"Total projects: {uniqueProjects.Count}");

            foreach (var project in uniqueProjects.OrderBy(p => p))
            {
                Console.WriteLine("\t-" + project);
            }
        }

        private static string ResolveSolutionPath(string solutionPath) => 
            string.IsNullOrWhiteSpace(solutionPath) ? Directory.GetCurrentDirectory() : solutionPath;

        private static async Task CreateSolutionIfNotExists(string solutionName, string solutionPath)
        {
            var fullSolutionPath = GetFullSolutionPathWithName(solutionName, solutionPath);

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

        private static string GetFullSolutionPathWithName(string solutionName, string solutionPath)
        {
            var fullSolutionPath = Path.Combine(solutionPath, $"{solutionName}.sln");
            return fullSolutionPath;
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
                Arguments = arguments,
                RedirectStandardError = true
            };

            var process = new Process {StartInfo = startInfo};
            process.Start();
            
            var error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                
                throw new Exception(
                    $"received exit code {process.ExitCode} for command `{command} {arguments}` " +
                    $"in working directory `{workingDirectory}` \nerror: {error}");
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