using System.Collections.Generic;

namespace AsciiTreeDiagram
{
    public class Node
    {
        public string Name { get; set; }

        public List<Node> Children { get; } = new();
    }
}