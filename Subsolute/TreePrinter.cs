using System;

/*
 * Code shamelessly taken with small adjustments from
 * https://github.com/andrewlock/blog-examples/tree/master/AsciiTreeDiagram/AsciiTreeDiagram.
 * Described in Andrew Lock's blog post https://andrewlock.net/creating-an-ascii-art-tree-in-csharp/
 */
namespace AsciiTreeDiagram
{
    public class TreePrinter
    {
        // Constants for drawing lines and spaces
        private const string Cross = " ├─";
        private const string Corner = " └─";
        private const string Vertical = " │ ";
        private const string Space = "    ";
        
        internal void PrintNode(Node node, string indent = "")
        {
            Console.WriteLine(node.Name);

            // Loop through the children recursively, passing in the
            // indent, and the isLast parameter
            var childrenCount = node.Children.Count;
            for (var i = 0; i < childrenCount; i++)
            {
                var child = node.Children[i];
                var isLast = i == (childrenCount - 1);
                PrintChildNode(child, indent, isLast);
            }
        }

        private void PrintChildNode(Node node, string indent, bool isLast)
        {
            // Print the provided pipes/spaces indent
            Console.Write(indent);

            // Depending if this node is a last child, print the
            // corner or cross, and calculate the indent that will
            // be passed to its children
            if (isLast)
            {
                Console.Write(Corner);
                indent += Space;
            }
            else
            {
                Console.Write(Cross);
                indent += Vertical;
            }

            PrintNode(node, indent);
        }
    }
}