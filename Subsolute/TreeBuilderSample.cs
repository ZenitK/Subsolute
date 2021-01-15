using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AsciiTreeDiagram
{
    public class TreeBuilderSample
    {
        static void Main(string[] args)
        {
            // Get the list of nodes
            var topLevelNodes = CreateNodeList();
            
            var printer = new TreePrinter();
            
            foreach (var node in topLevelNodes)
            { 
                printer.PrintNode(node);
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to exit...");
                Console.Read();
            }
        }

        private static List<Node> CreateNodeList() =>
            new()
            {
                new()
                {
                    Name = "Default",
                    Children =
                    {
                        new Node
                        {
                            Name = "Package",
                            Children = {
                                new Node
                                {
                                    Name = "Zip-Files",
                                    Children = {
                                        new Node
                                        {
                                            Name = "Copy-Files",
                                            Children = {
                                                new Node
                                                {
                                                    Name = "Run-Unit-Tests",
                                                    Children = {
                                                        new Node
                                                        {
                                                            Name = "Build",
                                                            Children = {
                                                                new Node
                                                                {
                                                                    Name = "Restore-NuGet-Packages",
                                                                    Children = {
                                                                        new Node { Name = "Clean" }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },

                                new Node
                                {
                                    Name = "Create-Nuget-Packages",
                                    Children = {
                                        new Node
                                        {
                                            Name = "Copy-Files",
                                            Children = {
                                                new Node
                                                {
                                                    Name = "Run-Unit-Tests",
                                                    Children = {
                                                        new Node
                                                        {
                                                            Name = "Build",
                                                            Children = {
                                                                new Node
                                                                {
                                                                    Name = "Restore-NuGet-Packages",
                                                                    Children = {
                                                                        new Node { Name = "Clean" }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new()
                {
                    Name = "AppVeyor",
                    Children =
                    {
                        new Node
                        {
                            Name = "Upload-AppVeyor-Artifacts",
                            Children = {
                                new Node
                                {
                                    Name = "Sign-Binaries",
                                    Children = {
                                        new Node
                                        {
                                            Name = "Zip-Files",
                                            Children = {
                                                new Node
                                                {
                                                    Name = "Run-Unit-Tests",
                                                    Children = {
                                                        new Node
                                                        {
                                                            Name = "Build",
                                                            Children = {
                                                                new Node
                                                                {
                                                                    Name = "Restore-NuGet-Packages",
                                                                    Children = {
                                                                        new Node { Name = "Clean" }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },

                                new Node
                                {
                                    Name = "Create-Nuget-Packages",
                                    Children = {
                                        new Node
                                        {
                                            Name = "Copy-Files",
                                            Children = {
                                                new Node
                                                {
                                                    Name = "Run-Unit-Tests",
                                                    Children = {
                                                        new Node
                                                        {
                                                            Name = "Build",
                                                            Children = {
                                                                new Node
                                                                {
                                                                    Name = "Restore-NuGet-Packages",
                                                                    Children = {
                                                                        new Node { Name = "Clean" }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
    }

}