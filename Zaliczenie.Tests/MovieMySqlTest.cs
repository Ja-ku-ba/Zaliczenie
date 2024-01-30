using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Zaliczenie.Tests
{
    [TestClass]
    public class MovieMySqlTests
    {
        private HttpClient _httpClient;
        private string _id = "";
        private object _newElement = new
        {
            author = "Francis Ford Coppola",
            id = "",
            rating = "9",
            relased = new DateTime(1972, 3, 14, 10, 0, 0),
            title = "OJCIEC CHRZESTNY"
        };

        public MovieMySqlTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(_newElement), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("movie/mysql", content);
            Uri locationUri = response.Headers.Location;
            string[] segments = locationUri.Segments;
            string lastSegment = segments[segments.Length - 1];
            _id = lastSegment.Trim('/');
        }


        [TestMethod]
        public async Task PostMovie_ReturnsId_StatusCode()
        {
            Assert.AreNotEqual(null, _id);
        }

        [TestMethod]
        public async Task GetMovieById_RetursStatusCode()
        {
            if (_id != null)
            {
                var response = await _httpClient.GetAsync($"movie/mysql/{_id}");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
            else
            {
                Assert.Fail("Utwór z podanymi kryteriami nie istnieje");
            }
        }

        [TestMethod]
        public async Task UpdatesMovie_ReturnsStatusCode()
        {
            var updateModel = new
            {
                author = "Krzysztof Krawczyk",
                rating = "9",
                id = _id,
                relased = new DateTime(1976, 3, 2, 23, 0, 0),
                title = "Akademia Pana Kleksa"
            };

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updateModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"movie/mysql/{_id}", content);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task DeleteMovie_Returns()
        {
            var response = await _httpClient.DeleteAsync($"movie/mysql/{_id}");
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await _httpClient.DeleteAsync($"movie/mysql/{_id}");
        }
    }
}
