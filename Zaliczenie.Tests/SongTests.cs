using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Zaliczenie.Tests
{
    [TestClass]
    public class SongTests
    {
        private HttpClient _httpClient;
        private string _id = "";
        private object _newElement = new
        {
            author = "Goran Bregovi� i Krzysztof Krawczyk",
            id = "",
            rating = "8",
            relased = new DateTime(2003, 3, 1, 10, 0, 0),
            title = "M�j przyjacielu"
        };

        public SongTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(_newElement), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("song", content);
            Uri locationUri = response.Headers.Location;
            string[] segments = locationUri.Segments;
            string lastSegment = segments[segments.Length - 1];
            _id = lastSegment.Trim('/');
        }


        [TestMethod]
        public async Task PostSong_ReturnsId_StatusCode()
        {
            Assert.AreNotEqual(null, _id);
        }

        public async Task<string> CompareSongs_ReturnsIdOrNull()
        {
            var response = await _httpClient.GetAsync("song");
            var stringResult = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonConvert.DeserializeObject<List<dynamic>>(stringResult,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            dynamic dynamicElement = _newElement;
            foreach (var item in deserializedResponse)
            {
                if ((string)item.title == (string)dynamicElement.title && (string)item.author == (string)dynamicElement.author)
                {
                    if ((string)item.rating == (string)dynamicElement.rating)
                    {
                        return item.id;
                    }
                }
            }
            return null;
        }

        [TestMethod]
        public async Task GetSongById_RetursStatusCode()
        {
            if (_id != null)
            {
                var response = await _httpClient.GetAsync($"song/{_id}");

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
            else
            {
                Assert.Fail("Utw�r z podanymi kryteriami nie istnieje");
            }
        }

        [TestMethod]
        public async Task UpdatesSong_ReturnsStatusCode()
        {
            var updateModel = new
            {
                author = "Maciej Male�czuk & Sentino feat. Yugopolis",
                rating = "7",
                id = _id,
                relased = new DateTime(2021, 12, 30, 23, 0, 0),
                title = "Ostatnia nocka ale to DRILL"
            };

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updateModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"song/{_id}", content);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteSong_Returns()
        {

            var response = await _httpClient.DeleteAsync($"song/{_id}");
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await _httpClient.DeleteAsync($"song/{_id}");
        }
    }
}
