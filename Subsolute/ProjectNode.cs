using System.Collections.Generic;

namespace AsciiTreeDiagram
{
    public record ProjectNode(string Name, string AbsolutePath, List<ProjectNode> Children = null);
}