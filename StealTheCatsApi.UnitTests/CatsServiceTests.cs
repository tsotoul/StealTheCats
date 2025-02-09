using AutoMapper;
using NSubstitute;
using StealTheCatsApi.Dtos;
using StealTheCatsApi.Models;
using StealTheCatsApi.Repositories.Interfaces;
using StealTheCatsApi.Services;

namespace StealTheCatsApi.UnitTests
{
    [TestFixture]
    public class CatsServiceTests
    {
        private ICatsApiRepository _catsApiRepository;
        private IDatabaseRepository _databaseRepository;
        private IMapper _mapper;
        private CatsService _catsService;

        [SetUp]
        public void Setup()
        {
            _catsApiRepository = Substitute.For<ICatsApiRepository>();
            _databaseRepository = Substitute.For<IDatabaseRepository>();
            _mapper = Substitute.For<IMapper>();
            _catsService = new CatsService(_catsApiRepository, _databaseRepository, _mapper);
        }

        [Test]
        public async Task FetchCatsAsync_Should_Call_GetCatsAsync()
        {
            // Arrange
            var catDtos = new List<ApiCatDto>
            {
                new ApiCatDto { Id = "1", Url = "http://test.com/1", Height = 500, Width = 600 },
                new ApiCatDto { Id = "2", Url = "http://test.com/2", Height = 600, Width = 700 }
            };
            var cats = new List<Cat>
            {
                new Cat { CatId = "1", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 500, Width = 600 },
                new Cat { CatId = "2", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 600, Width = 700 }
            };

            _catsApiRepository.GetCatsAsync(Arg.Any<int>()).Returns(catDtos);
            _catsApiRepository.DownloadImageAsync(Arg.Any<string>()).Returns(new byte[] { 0x00, 0x03, 0x04, 0x05 });
            _mapper.Map<List<Cat>>(Arg.Any<List<ApiCatDto>>()).Returns(cats);
            _databaseRepository.SaveCatAsync(Arg.Any<Cat>()).Returns(Task.FromResult(true));

            // Act
            await _catsService.FetchCatsAsync(2);

            // Assert
            await _catsApiRepository.Received(1).GetCatsAsync(2);
            await _databaseRepository.Received(2).SaveCatAsync(Arg.Any<Cat>());
        }

        [Test]
        public async Task FetchCatsAsync_Should_Retry_WhenSavedLessThanRequiredCats()
        {
            // Arrange
            var catDtos1 = new List<ApiCatDto>
            {
                new ApiCatDto { Id = "1", Url = "http://test.com/1", Height = 500, Width = 600 }
            };
            var cats = new List<Cat>
            {
                new Cat { CatId = "1", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 500, Width = 600 }
            };

            _catsApiRepository.GetCatsAsync(Arg.Any<int>()).Returns(catDtos1);
            _catsApiRepository.DownloadImageAsync(Arg.Any<string>()).Returns(new byte[] { 0x00, 0x03, 0x04, 0x05 });
            _mapper.Map<List<Cat>>(Arg.Any<List<ApiCatDto>>()).Returns(cats);
            _databaseRepository.SaveCatAsync(Arg.Any<Cat>()).Returns(Task.FromResult(true));

            // Act
            await _catsService.FetchCatsAsync(2);

            // Assert
            await _catsApiRepository.Received(1).GetCatsAsync(2);
            await _catsApiRepository.Received(1).GetCatsAsync(1);
            await _databaseRepository.Received(2).SaveCatAsync(Arg.Any<Cat>());
        }

        [Test]
        public async Task GetCatByIdAsync_ShouldReturnMappedCat()
        {
            var cat = new Cat { CatId = "1", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 500, Width = 600 };
            var databaseCatDto = new DatabaseCatDto
            {
                Id = 1,
                CatId = "TestCatId1",
                Width = 500,
                Height = 600,
                Image = new byte[] { 0x00, 0x01, 0x02, 0x03 },
                Created = new DateTime(2025, 2, 8)
            };
            _databaseRepository.GetCatByIdAsync(Arg.Any<int>()).Returns(cat);
            _mapper.Map<DatabaseCatDto>(cat).Returns(databaseCatDto);

            var result = await _catsService.GetCatByIdAsync(1);

            Assert.That(result, Is.EqualTo(databaseCatDto));
        }

        [Test]
        public async Task GetCatsAsync_ShouldReturnMappedCats()
        {
            var cats = new List<Cat>
            {
                new Cat { CatId = "1", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 500, Width = 600 },
                new Cat { CatId = "2", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 600, Width = 700 }
            };
            var databaseCatDtos = new List<DatabaseCatDto>
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
            _databaseRepository.GetCatsAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(Task.FromResult((IEnumerable<Cat>)cats));
            _mapper.Map<IEnumerable<DatabaseCatDto>>(cats).Returns(databaseCatDtos);

            var result = await _catsService.GetCatsAsync(1, 10);

            Assert.That(result, Is.EqualTo(databaseCatDtos));
        }

        [Test]
        public async Task GetCatsByTagAsync_ShouldReturnMappedCats()
        {
            var cats = new List<Cat>
            {
                new Cat { CatId = "1", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 500, Width = 600 },
                new Cat { CatId = "2", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 600, Width = 700 }
            };
            var databaseCatDtos = new List<DatabaseCatDto>
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
            _databaseRepository.GetCatsByTagAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>()).Returns(Task.FromResult((IEnumerable<Cat>)cats));
            _mapper.Map<IEnumerable<DatabaseCatDto>>(cats).Returns(databaseCatDtos);

            var result = await _catsService.GetCatsByTagAsync("cute", 1, 10);

            Assert.That(result, Is.EqualTo(databaseCatDtos));
        }
    }
}
