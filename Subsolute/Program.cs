using System;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Subsolute
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // Used implicitly
    public class Options
    {
        [Option(
            'p',
            "projects",
            Required = true,
            HelpText = "Full path to csproj|fsproj files.")]
        public string[] ProjectPaths { get; set; }

        [Option(
            "sln-path",
            Required = false,
            HelpText = "Full path for the new solution file. If not set it will be the current execution directory.",
            Default = null)]
        public string SolutionPath { get; set; }

        [Option(
            'n',
            "sln-name",
            Required = false,
            HelpText = "Solution name without the file extension. " +
                       "By default, the solution name is the same as the working directory name.")]
        public string SolutionName { get; set; }

        [Option(
            'v',
            "verbose",
            Required = false,
            HelpText = "Print project dependency trees")]
        public bool IsVerbose => true;
    }

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var parser = new Parser(config =>
            {
                config.AutoVersion = false;
                config.AutoHelp = true;
                config.HelpWriter = Console.Out;
            });

            var parserResult = parser.ParseArguments<Options>(args);

            await parserResult.WithParsedAsync(async o =>
            {
                var treeBuilder = new TreeBuilder();
                var projectTrees = treeBuilder.BuildProjectTree(o.ProjectPaths).ToList();

                if (o.IsVerbose)
                {
                    var treePrinter = new TreePrinter();

                    foreach (var projectTree in projectTrees)
                    {
                        treePrinter.PrintNode(projectTree);
                    }
                }

                var builder = new SolutionBuilder();
                await builder.Build(projectTrees.First(), o.SolutionName, o.SolutionPath);
            });
        }
    }
}