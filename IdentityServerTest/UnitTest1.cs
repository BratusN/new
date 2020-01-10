using IdentityModel.Client;
using IdentityServerTest;
using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class Tests
    {
        private readonly HttpClient _Client;
        private readonly string _TargetUrl;

        public Tests()
        {
            _Client = TestHelper.CreateClient();
            _TargetUrl = TestHelper.TargetUrl;
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task DiscoverTest()
        {
            var response = await _Client.GetAsync("account/login");
            Assert.True(response.IsSuccessStatusCode);

            var disco = await _Client.GetDiscoveryDocumentAsync(_TargetUrl);
            Assert.False(disco.IsError);
        }

        [Test]
        public async Task GetReportTest()
        {
            var content = new StringContent("",
                Encoding.UTF8, "application/json");
            var response = await _Client.PostAsync("api/UserActivity/getReport", content);
            //Assert.True(response.IsSuccessStatusCode);
            //var responseString = await response.Content.ReadAsStringAsync();
            //var responseReport = (List<ReportDto>)TestHelper.Deserialize(responseString, typeof(List<ReportDto>));
            //Assert.NotNull(responseReport);
            //Assert.AreEqual(5, responseReport.Count);
            //Assert.AreEqual(25, responseReport.First(x => x.QuestItemId == 1).RespondentCount);
            //Assert.AreEqual(5, responseReport.First(x => x.QuestItemId == 5).RespondentCount);
        }
    }
}