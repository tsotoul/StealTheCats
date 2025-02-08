using StealTheCats.Dtos;

namespace StealTheCats.Repositories.Interfaces
{
    public interface ICatsApiRepository
    {
        Task<IEnumerable<ApiCatDto>> GetCatsAsync(int numberOfCatsToFetch);
    }
}
