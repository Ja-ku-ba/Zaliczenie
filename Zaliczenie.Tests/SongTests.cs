using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using DnsClient;

namespace Zaliczenie.Tests
{
    [TestClass]
    public class ApiTests
    {
        private HttpClient _httpClient;
        private object _newElement = new
        {
            author = "Goran Bregoviæ i Krzysztof Krawczyk",
            id = "",
            rating = "10",
            relased = new DateTime(2003, 3, 2, 23, 0, 0),
            title = "Mój przyjacielu"
        };

        public ApiTests() { 
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        [TestMethod]
        public async Task PostSong_ReturnsStatusCode_Created()
        {

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(_newElement), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("song", content);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
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
        public async Task GetSong_CheksIfExists_ReturnsStatusCode_200Ok()
        {
            var checkIfExists = CompareSongs_ReturnsIdOrNull();
            Assert.AreNotEqual(checkIfExists, null);
        }

        [TestMethod]
        public async Task GetSongById_ReturnsBool()
        {
            var getId = CompareSongs_ReturnsIdOrNull();
            if (getId != null)
            {
                var response = await _httpClient.GetAsync($"song/{getId}");

                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            } else
            {
                Assert.Fail("Utwór z podanymi kryteriami nie istnieje");
            }
        }

        [TestMethod]
        private async Task UpdatesSong_ReturnsStatusCode()
        {
            var updateModel = new {
                author = "Krzysztof Krawczyk",
                id = "",
                rating = "9",
                relased = new DateTime(1976, 3, 2, 23, 0, 0),
                title = "Parastatek"
            };

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updateModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("song", content);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task DeleteSong_Returns()
        {
            var getId = await CompareSongs_ReturnsIdOrNull();
            if (getId != null)
            {
                var response = await _httpClient.DeleteAsync($"song/{getId}");

                // Check if the deletion is successful
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
            else
            {
                Assert.Fail("Utwór z danym id nie istnieje");
            }
        }
    }
}
