using System;
using AsciiTreeDiagram;

namespace Subsolute
{
    public static class Program
    {
        private const string Usage = "Usage: subsolute <project-path>";

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Usage);
                Environment.Exit(-1); 
            }
            
            var projectPath = args[0];
            
            var sb = new TreeBuilder();
            var projectTree = sb.BuildProjectTree(projectPath);

            var treePrinter = new TreePrinter();
            treePrinter.PrintNode(projectTree);
        }
    }
}