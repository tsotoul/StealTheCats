using Microsoft.EntityFrameworkCore;
using Shouldly;
using StealTheCatsApi.Models;
using StealTheCatsApi.Repositories;

namespace StealTheCatsApi.UnitTests
{
    [TestFixture]
    public class DatabaseRepositoryTests
    {
        private CatsDbContext _context;
        private DatabaseRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CatsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new CatsDbContext(options);
            _repository = new DatabaseRepository(_context);

            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task SaveCatAsync_ShouldReturnFalse_WhenCatExistInTheDb()
        {
            // Arrange
            var cat = new Cat { CatId = "1", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 500, Width = 600 };
            await _repository.SaveCatAsync(cat);

            // Act
            var result = await _repository.SaveCatAsync(cat);

            // Assert
            result.ShouldBeFalse();
            _context.Cats.Count().ShouldBe(1);
        }

        [Test]
        public async Task SaveCatAsync_ShouldSaveCatAndTagsSuccessfully_AndReturnTrue_WhenCatDoesNotExistInTheDb()
        {
            // Arrange
            var cat = new Cat { CatId = "1", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 500, Width = 600, Temperaments = ["Calm,", "Quiet"] };

            // Act
            var result = await _repository.SaveCatAsync(cat);

            // Assert
            result.ShouldBeTrue();
            _context.Cats.Count().ShouldBe(1);
            _context.Tags.Count().ShouldBe(2);
        }

        [Test]
        public async Task SaveCatAsync_ShouldSaveCatTagSuccessfully_WhenCatTagDoesNotExistInTheDb()
        {
            // Arrange
            var cat = new Cat { CatId = "1", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 500, Width = 600, Temperaments = ["Calm", "Quiet"] };

            // Act
            var result = await _repository.SaveCatAsync(cat);

            // Assert
            result.ShouldBeTrue();
            _context.Cats.Count().ShouldBe(1);
            _context.Tags.Count().ShouldBe(2);
            _context.CatTags.Count().ShouldBe(2);
        }

        [Test]
        public async Task SaveCatAsync_ShouldNotAddTag_WhenTagExistInTheDb()
        {
            // Arrange
            var cat = new Cat { CatId = "1", Image = new byte[] { 0x00, 0x01, 0x02, 0x03 }, Height = 500, Width = 600, Temperaments = ["Calm", "Quiet"] };
            await _context.Tags.AddAsync(new Tag { Name = "Calm" });
            await _context.Tags.AddAsync(new Tag { Name = "Funny" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.SaveCatAsync(cat);

            // Assert
            result.ShouldBeTrue();
            _context.Cats.Count().ShouldBe(1);
            _context.Tags.Count().ShouldBe(3);
            _context.CatTags.Count().ShouldBe(2);
        }
    }
}
