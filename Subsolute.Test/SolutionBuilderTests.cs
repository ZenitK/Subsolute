using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using static Subsolute.Test.TestConstants;

namespace Subsolute.Test
{
    [TestFixture]
    public class SolutionBuilderTests
    {
        private const string TestSolutionDir = "./TestSolutions";
        private readonly SolutionBuilder _builder = new();
        private string _solutionName;

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedMember.Global
        public TestContext TestContext { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Directory.CreateDirectory(TestSolutionDir);
        }

        [SetUp]
        public void Setup()
        {
            _solutionName = TestContext.CurrentContext.Test.Name;
        }

        [Test]
        public async Task Test_CreateBasicSolution()
        {
            await _builder.Build(DummyProjectNode, _solutionName, TestSolutionDir);

            var fullSolutionPath = GetFullSolutionPath(_solutionName);

            Assert.That(File.Exists(fullSolutionPath));

            var solutionContent = GetSolutionContent(fullSolutionPath);
            Assert.That(solutionContent, Contains.Substring("Microsoft Visual Studio"));
            Assert.That(solutionContent, Contains.Substring("Global"));
        }

        [Test]
        public async Task Test_CreateBasicSolutionWithOneProject()
        {
            var projectNode = new ProjectNode("SampleProject", Path.GetFullPath(SimpleSampleProjectPath));
            await _builder.Build(projectNode, _solutionName, TestSolutionDir);

            Assert.That(GetSolutionContent(), Contains.Substring(SimpleSampleProjectName));
        }

        [Test]
        public async Task Test_CreateSolutionWithMultipleProjects()
        {
            var firstProject = new ProjectNode("FirstProject", Path.GetFullPath(SimpleSampleProjectPath));
            var secondProject = new ProjectNode("SecondProject", Path.GetFullPath(ComplexSampleProjectPath));

            await _builder.Build(new[] {firstProject, secondProject}, _solutionName, TestSolutionDir);

            var solutionContent = GetSolutionContent();
            Assert.That(solutionContent, Contains.Substring("SampleProjectWithoutDependencies"));
            Assert.That(solutionContent, Contains.Substring("SampleRootApp"));
        }

        private static string GetSolutionContent(string fullSolutionPath) => File.ReadAllText(fullSolutionPath);
        private string GetSolutionContent() => File.ReadAllText(GetFullSolutionPath(_solutionName));

        private static string GetFullSolutionPath(string solutionName) =>
            Path.Combine(TestSolutionDir, $"{solutionName}.sln");

        private static ProjectNode DummyProjectNode => new("Whatever", string.Empty);

        [OneTimeTearDown]
        public void TearDown()
        {
            var directoryInfo = new DirectoryInfo(TestSolutionDir);

            // We still need to delete each file manually for some reason...
            foreach (var file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }

            Directory.Delete(TestSolutionDir, recursive: true);
        }
    }
}