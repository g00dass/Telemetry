using Telemetry.Client.Api;
using Xunit;
using Xunit.Abstractions;

namespace Sandbox
{
    public class StatisticsApiTests
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly StatisticsApi client = new StatisticsApi("https://localhost:32775/");

        public StatisticsApiTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        void GetAll()
        {
            client.StatisticsApiAppInfoAllGet().ForEach(x => testOutputHelper.WriteLine(x.ToJson()));
        }
    }
}
