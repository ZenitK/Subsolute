using System.Threading.Tasks;
using AsciiTreeDiagram;

namespace Subsolute
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var projectPath = args[0];
            
            var sb = new SolutionBuilder();
            var projectTree = sb.BuildProjectTree(projectPath);

            var treePrinter = new TreePrinter();
            treePrinter.PrintNode(projectTree);
        }
    }
}