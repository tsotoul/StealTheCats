using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using StealTheCatsApi.Configuration;
using StealTheCatsApi.Dtos;
using StealTheCatsApi.Repositories;
using StealTheCatsApi.Repositories.Interfaces;
using System.Net;
using System.Text.Json;

namespace StealTheCatsApi.UnitTests
{
    [TestFixture]
    public class CatsApiRepositoryTests
    {
        private IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private IOptions<AppSettings> _options;
        private MockHttpMessageHandler _mockHttpMessageHandler;
        private ICatsApiRepository _repository;

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new MockHttpMessageHandler();

            _httpClient = new HttpClient(_mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://api.thecatapi.com")
            };

            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            _httpClientFactory.CreateClient().Returns(_httpClient);
            var _options = Options.Create(new AppSettings()
            {
                CatsApiUrl = "https://api.thecatapi.com/v1/images/search",
                ApiKey = "testKey",
                ApiSecret = "testSecret"
            });

            _repository = new CatsApiRepository(_httpClientFactory, _options);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
            _mockHttpMessageHandler.Dispose();
        }

        [Test]
        public async Task GetCatsAsync_ShouldReturnCats_WhenApiResponseIsSuccessful()
        {
            // Arrange
            var catDtos = new List<ApiCatDto>
            {
                new ApiCatDto { Id = "1", Url = "http://testImage.com/1", Height = 500, Width = 600 },
                new ApiCatDto { Id = "2", Url = "http://testImage.com/2", Height = 600, Width = 700 }
            };
            var jsonResponse = JsonSerializer.Serialize(catDtos);
            _mockHttpMessageHandler.SetResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

            // Act
            var result = await _repository.GetCatsAsync(1);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<ApiCatDto>>();

            result.ShouldContain(c => c.Id == "1");

            Assert.That(result, Is.Not.Null);
            result.ShouldBeOfType<List<ApiCatDto>>();
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.ElementAt(0).Id, Is.EqualTo("1"));
            Assert.That(result.ElementAt(0).Url, Is.EqualTo("http://testImage.com/1"));
            Assert.That(result.ElementAt(0).Height, Is.EqualTo(500));
            Assert.That(result.ElementAt(0).Width, Is.EqualTo(600));
            Assert.That(result.ElementAt(1).Id, Is.EqualTo("2"));
            Assert.That(result.ElementAt(1).Url, Is.EqualTo("http://testImage.com/2"));
            Assert.That(result.ElementAt(1).Height, Is.EqualTo(600));
            Assert.That(result.ElementAt(1).Width, Is.EqualTo(700));
        }

        [Test]
        public void GetCatsAsync_ShouldThrowException_WhenApiReturnsFailureStatusCode()
        {
            // Arrange
            _mockHttpMessageHandler.SetResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

            // Act and Assert
            var exception = Should.ThrowAsync<Exception>(() => _repository.GetCatsAsync(1));
            exception.Result.Message.ShouldContain("Something went wrong when trying to fetch the cats from the Api:");
        }

        [Test]
        public void GetCatsAsync_ShouldThrowException_WhenApiResponseIsInvalid()
        {
            // Arrange
            _mockHttpMessageHandler.SetResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Invalid JSON")
            });

            // Act and Assert
            var exception = Should.ThrowAsync<Exception>(() => _repository.GetCatsAsync(1));
            exception.Result.Message.ShouldContain("Something went wrong when trying to fetch the cats from the Api:");
        }

        [Test]
        public async Task DownloadImageAsync_ShouldReturnByteArray_WhenDownloadIsSuccessful()
        {
            // Arrange
            var fakeImageData = new byte[] { 1, 2, 3, 4, 5 };
            _mockHttpMessageHandler.SetResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(fakeImageData)
            });

            // Act
            var result = await _repository.DownloadImageAsync("http://test.com/image.jpg");

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBe(fakeImageData);
        }

        [Test]
        public void DownloadImageAsync_ShouldThrowException_WhenDownloadFails()
        {
            // Arrange
            _mockHttpMessageHandler.SetResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

            // Act and Assert
            var exception = Should.ThrowAsync<Exception>(() => _repository.DownloadImageAsync("http://test.com/image.jpg"));
            exception.Result.Message.ShouldContain("Error downloading the image");
        }
    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage _response;

        public void SetResponse(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}
