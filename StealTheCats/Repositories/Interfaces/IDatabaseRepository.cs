using StealTheCats.Models;

namespace StealTheCats.Repositories.Interfaces
{
    public interface IDatabaseRepository
    {
        Task SaveCatAsync(Cat cat);
    }
}
