using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Subsolute.Test
{
    [TestFixture]
    public class TreeBuildTests
    {
        private const string SimpleSampleProjectPath =
            "./SampleProjects/SampleProjectWithoutDependencies/SampleProjectWithoutDependencies.csproj";

        private const string ComplexSampleProjectPath =
            "./SampleProjects/SampleRootApp/SampleRootApp.csproj";

        private const string SampleRootAppDep1Level1 = "SampleRootAppDep1Level1.csproj";
        private const string SampleRootAppDep2Level1 = "SampleRootAppDep2Level1.csproj";
        private const string SampleRootAppDep2Level2 = "SampleRootAppDep2Level2.csproj";

        private readonly TreeBuilder _treeBuilder = new();

        [Test]
        public void Test_FileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() => _treeBuilder.BuildProjectTree("some illegal path"));
        }

        [Test]
        public void Test_NoExceptionIsThrown()
        {
            Assert.DoesNotThrow(() => _treeBuilder.BuildProjectTree(SimpleSampleProjectPath));
        }

        [Test]
        public void Test_ProjectWithoutDependencies()
        {
            var projectTree = _treeBuilder.BuildProjectTree(SimpleSampleProjectPath);

            Assert.That(projectTree.Name, Is.EqualTo("SampleProjectWithoutDependencies.csproj"));
        }

        [Test]
        public void Test_ProjectWithDependencies_AllLevels()
        {
            var projectTree = _treeBuilder.BuildProjectTree(ComplexSampleProjectPath);

            Assert.That(projectTree.Children.Count, Is.EqualTo(2));

            CollectionAssert.AreEquivalent(
                new[] {SampleRootAppDep1Level1, SampleRootAppDep2Level1},
                projectTree.Children.Select(x => x.Name));

            var level2Project = projectTree
                .Children
                .Single(x => x.Name == SampleRootAppDep1Level1)
                .Children
                .Single()
                .Name;

            Assert.That(level2Project, Is.EqualTo(SampleRootAppDep2Level2));
        }
    }
}