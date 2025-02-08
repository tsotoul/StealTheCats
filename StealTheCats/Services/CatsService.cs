using AutoMapper;
using StealTheCats.Dtos;
using StealTheCats.Models;
using StealTheCats.Repositories.Interfaces;
using StealTheCats.Services.Interfaces;

namespace StealTheCats.Services
{
    public class CatsService : ICatsService
    {
        private readonly ICatsApiRepository _catsApiRepository;
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IMapper _mapper;

        public CatsService(ICatsApiRepository catsApiRepository, IDatabaseRepository databaseRepository, IMapper mapper)
        {
            _catsApiRepository = catsApiRepository;
            _databaseRepository = databaseRepository;
            _mapper = mapper;
        }

        public async Task FetchCatsAsync(int numberOfCatsToSave)
        {
            var numberOfCatsSaved = 0;
            var catsDtos = await _catsApiRepository.GetCatsAsync(numberOfCatsToSave);

            var catsFromTheApi = _mapper.Map<List<Cat>>(catsDtos);

            foreach (var cat in catsFromTheApi)
            {
                if (await _databaseRepository.SaveCatAsync(cat)) numberOfCatsSaved++;
            }

            if (numberOfCatsSaved < numberOfCatsToSave)
            {
                await FetchCatsAsync(numberOfCatsToSave - numberOfCatsSaved);
            }
        }

        public async Task<IEnumerable<DatabaseCatDto>> GetCatsAsync(int page, int pageSize)
        {
            var catsFromTheDatabase = await _databaseRepository.GetCatsAsync(page, pageSize);

            return _mapper.Map<IEnumerable<DatabaseCatDto>>(catsFromTheDatabase);
        }

        public async Task<IEnumerable<DatabaseCatDto>> GetCatsByTagAsync(string tag, int page, int pageSize)
        {
            var catsFromTheDatabase = await _databaseRepository.GetCatsByTagAsync(tag, page, pageSize);

            return _mapper.Map<IEnumerable<DatabaseCatDto>>(catsFromTheDatabase);
        }
    }
}
