using System.IO;
using NUnit.Framework;

namespace Subsolute.Test
{
    [TestFixture]
    public class TreeBuildTests
    {
        private const string SimpleSampleProjectPath = "./SampleProjects/SampleProjectWithoutDependencies.csproj";
        private readonly SolutionBuilder _solutionBuilder = new();

        [Test]
        public void Test_FileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() => _solutionBuilder.BuildProjectTree("some illegal path"));
        }
        
        [Test]
        public void Test_NoExceptionIsThrown()
        {
            Assert.DoesNotThrow(() => _solutionBuilder.BuildProjectTree(SimpleSampleProjectPath));
        }

        [Test]
        public void TestOneNodeTree()
        {
            var projectTree = _solutionBuilder.BuildProjectTree(SimpleSampleProjectPath);
            
            Assert.That(projectTree.Name, Is.EqualTo("SampleProjectWithoutDependencies.csproj"));
        }
    }
}