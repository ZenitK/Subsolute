using System.IO;
using System.Linq;
using NUnit.Framework;
using static Subsolute.Test.TestConstants;
// ReSharper disable IteratorMethodResultIsIgnored
// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace Subsolute.Test
{
    [TestFixture]
    public class TreeBuildTests
    {
        private readonly TreeBuilder _treeBuilder = new();

        [Test]
        public void Test_FileNotFoundException() => 
            Assert.Throws<FileNotFoundException>(() => _treeBuilder.BuildProjectTree("some illegal path").Single());

        [Test]
        public void Test_NoExceptionIsThrown() => 
            Assert.DoesNotThrow(() => _treeBuilder.BuildProjectTree(SimpleSampleProjectPath));

        [Test]
        public void Test_ProjectWithoutDependencies()
        {
            var projectTree = _treeBuilder.BuildProjectTree(SimpleSampleProjectPath).Single();

            Assert.That(projectTree.Name, Is.EqualTo("SampleProjectWithoutDependencies.csproj"));
        }
        
        [Test]
        public void Test_MultipleProjects()
        {
            var projectTrees = _treeBuilder.BuildProjectTree(SimpleSampleProjectPath, ComplexSampleProjectPath).ToList();
            Assert.That(projectTrees.Count, Is.EqualTo(2));
            CollectionAssert.AllItemsAreNotNull(projectTrees);
        }

        [Test]
        public void Test_ProjectWithDependencies_AllLevels()
        {
            var (_, _, projectNodes) = _treeBuilder.BuildProjectTree(ComplexSampleProjectPath).Single();

            Assert.That(projectNodes.Count, Is.EqualTo(2));

            CollectionAssert.AreEquivalent(
                new[] {SampleRootAppDep1Level1, SampleRootAppDep2Level1},
                projectNodes.Select(x => x.Name));

            var level2Project = projectNodes
                .Single(x => x.Name == SampleRootAppDep1Level1)
                .Children
                .Single()
                .Name;

            Assert.That(level2Project, Is.EqualTo(SampleRootAppDep2Level2));
        }
    }
}