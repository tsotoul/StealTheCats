using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using StealTheCatsApi.Controllers;
using StealTheCatsApi.Dtos;
using StealTheCatsApi.Services.Interfaces;
using System.Net;

namespace StealTheCatsApi.UnitTests
{
    [TestFixture]
    public class ControllerTests
    {
        private ICatsService _catsService;
        private CatsController _controller;

        [SetUp]
        public void Setup()
        {
            _catsService = Substitute.For<ICatsService>();
            _controller = new CatsController(_catsService);
        }

        [Test]
        public void FetchCatsAsync_ShouldHave_CorrectAttributes()
        {
            var methodInfo = typeof(CatsController).GetMethod(nameof(CatsController.FetchCatsAsync));
            var httpPostAttribute = methodInfo.GetAttribute<HttpPostAttribute>();

            // Assert
            Assert.That(httpPostAttribute, Is.Not.Null);
        }

        [Test]
        public async Task FetchCatsAsync_Should_Return201_When_Successful()
        {
            var response = await _controller.FetchCatsAsync();
            var result = response.ShouldBeOfType<CreatedResult>();

            // Assert
            result.StatusCode.ShouldBe((int)HttpStatusCode.Created);
        }

        [Test]
        public void GetCatByIdAsync_ShouldHave_CorrectAttributes()
        {
            var methodInfo = typeof(CatsController).GetMethod(nameof(CatsController.GetCatByIdAsync));
            var httpGetAttribute = methodInfo.GetAttribute<HttpGetAttribute>();

            // Assert
            Assert.That(httpGetAttribute, Is.Not.Null);
        }

        [Test]
        public async Task GetCatByIdAsync_Should_Return200_When_Successful()
        {
            // Arrange
            var cat = new DatabaseCatDto
            {
                Id = 1,
                CatId = "TestCatId",
                Width = 500,
                Height = 600,
                Image = new byte[] { 0x00, 0x01, 0x02, 0x03 },
                Created = new DateTime(2025, 2, 8)
            };

            _catsService.GetCatByIdAsync(1).Returns(Task.FromResult(cat));

            // Act
            var result = await _controller.GetCatByIdAsync(1);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            okResult.Value.ShouldBe(cat);
            await _catsService.Received(1).GetCatByIdAsync(1);
        }

        [Test]
        public async Task GetCatByIdAsync_CatNotFoundInDatabase_ReturnsNotFound()
        {
            // Arrange
            _catsService.GetCatByIdAsync(1).Returns(Task.FromResult<DatabaseCatDto>(null));

            // Act
            var result = await _controller.GetCatByIdAsync(1);

            // Assert
            result.ShouldBeOfType<NotFoundObjectResult>();
            var notFoundResult = (NotFoundObjectResult)result;
            notFoundResult.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
            notFoundResult.Value.ShouldBe("Cat not found in the database.");
            await _catsService.Received(1).GetCatByIdAsync(1);
        }

        [Test]
        public async Task GetCatByIdAsync_NegativeId_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetCatByIdAsync(-1);

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            var badRequestResult = (BadRequestObjectResult)result;
            badRequestResult.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            badRequestResult.Value.ShouldBe("CatId much be provided.");
            await _catsService.DidNotReceive().GetCatByIdAsync(Arg.Any<int>());
        }

        [Test]
        public void GetCatsAsync_ShouldHave_CorrectAttributes()
        {
            var methodInfo = typeof(CatsController).GetMethod(nameof(CatsController.GetCatsAsync));
            var httpGetAttribute = methodInfo.GetAttribute<HttpGetAttribute>();

            // Assert
            Assert.That(httpGetAttribute, Is.Not.Null);
        }

        [Test]
        public async Task GetCatsAsync_Should_Return200_WithValidRequestWithoutTag()
        {
            // Arrange
            var cats = new List<DatabaseCatDto>
            {
                new DatabaseCatDto
                {
                    Id = 1,
                    CatId = "TestCatId1",
                    Width = 500,
                    Height = 600,
                    Image = new byte[] { 0x00, 0x01, 0x02, 0x03 },
                    Created = new DateTime(2025, 2, 8)
                },
                new DatabaseCatDto
                {
                    Id = 3,
                    CatId = "TestCatId3",
                    Width = 700,
                    Height = 8800,
                    Image = new byte[] { 0x00, 0x03, 0x04, 0x05 },
                    Created = new DateTime(2025, 2, 9)
                }
            };
            _catsService.GetCatsAsync(1, 10).Returns(cats);

            // Act
            var result = await _controller.GetCatsAsync(null, 1, 10);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            okResult.Value.ShouldBe(cats);
            await _catsService.Received(1).GetCatsAsync(1, 10);
            await _catsService.DidNotReceive().GetCatsByTagAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>());
        }

        [Test]
        public async Task GetCatsAsync_Should_Return200_WithValidRequestWithTag()
        {
            // Arrange
            var cats = new List<DatabaseCatDto>
            {
                new DatabaseCatDto
                {
                    Id = 1,
                    CatId = "TestCatId1",
                    Width = 500,
                    Height = 600,
                    Image = new byte[] { 0x00, 0x01, 0x02, 0x03 },
                    Created = new DateTime(2025, 2, 8)
                },
                new DatabaseCatDto
                {
                    Id = 3,
                    CatId = "TestCatId3",
                    Width = 700,
                    Height = 8800,
                    Image = new byte[] { 0x00, 0x03, 0x04, 0x05 },
                    Created = new DateTime(2025, 2, 9)
                }
            };
            _catsService.GetCatsByTagAsync("calm", 1, 10).Returns(cats);

            // Act
            var result = await _controller.GetCatsAsync("calm", 1, 10);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            okResult.Value.ShouldBe(cats);
            await _catsService.Received(1).GetCatsByTagAsync("calm", 1, 10);
            await _catsService.DidNotReceive().GetCatsAsync(Arg.Any<int>(), Arg.Any<int>());
        }

        [TestCase(0, 10)]
        [TestCase(2, 0)]
        public async Task GetCatsAsync_InvalidPageOrPageSize_ReturnsBadRequest(int page, int pageSize)
        {
            // Act
            var result = await _controller.GetCatsAsync(null, 0, 10);

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            var badRequestResult = (BadRequestObjectResult)result;
            badRequestResult.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            badRequestResult.Value.ShouldBe("Page and pageSize must be greater than 0.");
            await _catsService.DidNotReceive().GetCatsAsync(Arg.Any<int>(), Arg.Any<int>());
            await _catsService.DidNotReceive().GetCatsByTagAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>());
        }
    }
}