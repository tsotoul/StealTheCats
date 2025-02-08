using StealTheCatsApi.Dtos;

namespace StealTheCatsApi.Services.Interfaces
{
    public interface ICatsService
    {
        Task FetchCatsAsync(int numberOfCatsToSave = 25);
        Task<DatabaseCatDto> GetCatByIdAsync(string catId);
        Task<IEnumerable<DatabaseCatDto>> GetCatsAsync(int page, int pageSize);
        Task<IEnumerable<DatabaseCatDto>> GetCatsByTagAsync(string tag, int page, int pageSize);
    }
}
