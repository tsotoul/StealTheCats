using StealTheCats.Models;
using StealTheCats.Repositories.Interfaces;

namespace StealTheCats.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly CatsDbContext _context;

        public DatabaseRepository(CatsDbContext context)
        {
            _context = context;
        }

        public async Task SaveCatAsync(Cat cat)
        {
            var tags = cat.Temperaments.Select(name => new Tag { Name = name }).ToList();

            foreach (var tag in tags)
            {
                if (!_context.Tags.Any(t => t.Name == tag.Name))
                {
                    await _context.Tags.AddAsync(tag);
                }
            }

            await _context.Cats.AddAsync(cat);
            await _context.SaveChangesAsync();

            foreach (var tag in tags)
            {
                var existingTagId = _context.Tags.First(t => t.Name == tag.Name).Id;
                _context.CatTags.Add(new CatTag
                {
                    CatId = cat.Id,
                    TagId = existingTagId == 0 ? tag.Id : existingTagId
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
