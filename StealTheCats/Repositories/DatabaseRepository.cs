using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> SaveCatAsync(Cat cat)
        {
            if (await _context.Cats.AnyAsync(c => c.CatId == cat.CatId))
            {
                return false;
            }

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

            return true;
        }

        public async Task<IEnumerable<Cat>> GetCatsAsync(int page, int pageSize)
        {
            return await _context.Cats
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cat>> GetCatsByTagAsync(string tag, int page, int pageSize)
        {
            return await _context.Cats
                .Where(c => c.CatTags.Any(ct => ct.Tag.Name == tag))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
