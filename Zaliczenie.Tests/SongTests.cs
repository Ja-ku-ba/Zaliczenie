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
            author = "Goran Bregoviæ i Krzysztof Krawczyk",
            id = "",
            rating = "8",
            relased = new DateTime(2003, 3, 1, 10, 0, 0),
            title = "Mój przyjacielu"
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

            var response = await _httpClient.PostAsync("song/mongo", content);
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

        [TestMethod]
        public async Task GetSongById_RetursStatusCode()
        {
            if (_id != null)
            {
                var response = await _httpClient.GetAsync($"song/mongo/{_id}");

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
            else
            {
                Assert.Fail("Utwór z podanymi kryteriami nie istnieje");
            }
        }

        [TestMethod]
        public async Task UpdatesSong_ReturnsStatusCode()
        {
            var updateModel = new
            {
                author = "Maciej Maleñczuk & Sentino feat. Yugopolis",
                rating = "7",
                id = _id,
                relased = new DateTime(2021, 12, 30, 23, 0, 0),
                title = "Ostatnia nocka ale to DRILL"
            };

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updateModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"song/mongo/{_id}", content);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteSong_Returns()
        {

            var response = await _httpClient.DeleteAsync($"song/mongo/{_id}");
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await _httpClient.DeleteAsync($"song/{_id}");
        }
    }
}
