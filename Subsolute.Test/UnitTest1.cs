using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using static Subsolute.SolutionBuilder;

namespace Subsolute.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_FileNotFoundException()
        {
            Assert.ThrowsAsync<FileNotFoundException>(() => Build("some illegal path"));
        }
        
        [Test]
        public async Task Test_SimpleProject()
        {
            await Build("./SampleProjects/SampleProjectWithoutDependencies.csproj");
        }
    }
}