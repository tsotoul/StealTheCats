using StealTheCats.Dtos;

namespace StealTheCats.Services.Interfaces
{
    public interface ICatsService
    {
        Task FetchCatsAsync(int numberOfCatsToSave = 25);
        Task<IEnumerable<DatabaseCatDto>> GetCatsAsync(int page, int pageSize);
        Task<IEnumerable<DatabaseCatDto>> GetCatsByTagAsync(string tag, int page, int pageSize);
    }
}
