using System;
using System.Threading.Tasks;
using AsciiTreeDiagram;
using CommandLine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Subsolute
{
    public class Options
    {
        [Option(
            'p',
            "project",
            Required = true,
            HelpText = "Full path to the csproj|fsproj file.")]
        public string ProjectPath { get; set; }

        [Option(
            "sln-path",
            Required = false,
            HelpText = "Full path for the new solution file. If not set it will be the current execution directory.")]
        public string SolutionPath { get; set; }

        [Option(
            'n',
            "sln-name",
            Required = true,
            HelpText = "Solution name without the file extension")]
        public string SolutionName { get; set; }
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
                var projectTree = treeBuilder.BuildProjectTree(o.ProjectPath);
                var treePrinter = new TreePrinter();
                treePrinter.PrintNode(projectTree);
                var solutionBuilder = new SolutionBuilder();
                await solutionBuilder.Build(projectTree, o.SolutionName, o.SolutionPath);
            });
        }
    }
}