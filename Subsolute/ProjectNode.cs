using System.Collections.Generic;

namespace AsciiTreeDiagram
{
    public record ProjectNode(string Name, List<ProjectNode> Children, string AbsolutePath, string ProjectGuid);
}