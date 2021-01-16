using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AsciiTreeDiagram;

namespace Subsolute
{
    public class SolutionBuilder
    {
        public async Task Build(ProjectNode root, string solutionName = "", string solutionPath = ".")
        {
            var uniqueProjects = ExtractAllUniqueProjects(root);

            Console.WriteLine($"Total projects: {uniqueProjects.Count}");

            foreach (var allProject in uniqueProjects.OrderBy(p => p.Name))
            {
                Console.WriteLine("\t-" + allProject.Name);
            }

            await CreateSolutionIfNotExists(solutionName, solutionPath);
        }

        private static async Task CreateSolutionIfNotExists(string solutionName, string solutionPath = ".")
        {
            if (!File.Exists(Path.Combine(solutionPath, $"{solutionName}.sln")))
            {
                await CreateSolution(solutionName, solutionPath);
            }
            else
            {
                Console.WriteLine("Solution already exists. Skipping.");
            }
        }
        
        private static async Task CreateSolution(string filename, string solutionPath)
        {
            var arguments = string.IsNullOrWhiteSpace(solutionPath) ?
                "new sln" :
                $"new sln --name {filename}";
            
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WorkingDirectory = solutionPath ?? ".",
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "dotnet",
                Arguments = arguments
            };
            
            var process = new System.Diagnostics.Process {StartInfo = startInfo};
            process.Start();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                var message = $"Failed to create solution {filename}, received -1 exit code " +
                             $"from `dotnet new sln --name {filename}` in directory {solutionPath}";

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Environment.Exit(process.ExitCode);
            }
        }

        /// <summary>
        /// Many projects like core libraries repeat multiple times, those we don't need to import multiple times 
        /// </summary>
        private static IReadOnlySet<Project> ExtractAllUniqueProjects(ProjectNode root)
        {
            var flatTree = Flatten(root);

            return flatTree
                .Select(x => new Project(x.Name, x.AbsolutePath))
                .ToHashSet();
        }

        private static IEnumerable<ProjectNode> Flatten(ProjectNode projectNode) =>
            projectNode
                .Children
                .SelectMany(Flatten)
                .Concat(new[] {projectNode});

        // Using a new data structure in order to have a proper hash value, the children's list is in the way otherwise
        private record Project(string Name, string AbsolutePath);
    }
}