using StealTheCatsApi.Dtos;

namespace StealTheCatsApi.Repositories.Interfaces
{
    public interface ICatsApiRepository
    {
        Task<IEnumerable<ApiCatDto>> GetCatsAsync(int numberOfCatsToFetch);
    }
}
