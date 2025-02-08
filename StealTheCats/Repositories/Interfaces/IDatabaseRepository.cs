using StealTheCats.Models;

namespace StealTheCats.Repositories.Interfaces
{
    public interface IDatabaseRepository
    {
        Task<bool> SaveCatAsync(Cat cat);
        Task<IEnumerable<Cat>> GetCatsAsync(int page, int pageSize);
        Task<IEnumerable<Cat>> GetCatsByTagAsync(string tag, int page, int pageSize);
    }
}
