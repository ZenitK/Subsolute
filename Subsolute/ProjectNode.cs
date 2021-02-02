using System.Collections.Generic;

namespace Subsolute
{
    public record ProjectNode(string Name, string AbsolutePath, List<ProjectNode> Children = null);
}