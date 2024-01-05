using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace Zaliczenie.Tests
{
    [TestClass]
    public class MovieTests
    {
        private HttpClient _httpClient;
        private object _newElement = new
        {
            author = "Francis Ford Coppola",
            id = "",
            rating = "9",
            relased = new DateTime(1972, 3, 14, 10, 0, 0),
            title = "OJCIEC CHRZESTNY"
        };

        public MovieTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }
        [TestMethod]
        public async Task PostMovie_ReturnsStatusCode_Created()
        {

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(_newElement), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("movie", content);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        public async Task<string> CompareMovies_ReturnsIdOrNull()
        {
            var response = await _httpClient.GetAsync("movie");
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
        public async Task GetMovie_CheksIfExists_ReturnsStatusCode_200Ok()
        {
            var checkIfExists = CompareMovies_ReturnsIdOrNull();
            Assert.AreNotEqual(checkIfExists, null);
        }

        [TestMethod]
        public async Task GetMovieById_RetursStatusCode()
        {
            var getId = CompareMovies_ReturnsIdOrNull();
            if (getId != null)
            {
                var response = await _httpClient.GetAsync($"movie/{getId}");

                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            }
            else
            {
                Assert.Fail("Utwór z podanymi kryteriami nie istnieje");
            }
        }

        [TestMethod]
        private async Task UpdatesMovie_ReturnsStatusCode()
        {
            var updateModel = new
            {
                author = "Krzysztof Krawczyk",
                id = "",
                rating = "9",
                relased = new DateTime(1976, 3, 2, 23, 0, 0),
                title = "Parastatek"
            };

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updateModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("movie", content);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task DeleteMovie_Returns()
        {
            var getId = await CompareMovies_ReturnsIdOrNull();
            if (getId != null)
            {
                var response = await _httpClient.DeleteAsync($"movie/{getId}");

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
