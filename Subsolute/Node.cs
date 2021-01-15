#nullable enable
using System.Collections.Generic;

namespace AsciiTreeDiagram
{
    public class Node
    {
        public string Name { get; set; }

        public List<Node> Children { get; set; }
        public string AbsolutePath { get; set; }
        public string? ProjectGuid { get; set; }
    }
}