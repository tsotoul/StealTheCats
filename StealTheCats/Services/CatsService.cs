using AutoMapper;
using StealTheCatsApi.Dtos;
using StealTheCatsApi.Models;
using StealTheCatsApi.Repositories.Interfaces;
using StealTheCatsApi.Services.Interfaces;

namespace StealTheCatsApi.Services
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

            List<Cat> catsFromTheApi = _mapper.Map<List<Cat>>(catsDtos);

            foreach (var cat in catsFromTheApi)
            {
                if (await _databaseRepository.SaveCatAsync(cat)) numberOfCatsSaved++;
            }

            if (numberOfCatsSaved < numberOfCatsToSave)
            {
                await FetchCatsAsync(numberOfCatsToSave - numberOfCatsSaved);
            }
        }

        public async Task<DatabaseCatDto> GetCatByIdAsync(int id)
        {
            var catFromTheDatabase = await _databaseRepository.GetCatByIdAsync(id);

            return _mapper.Map<DatabaseCatDto>(catFromTheDatabase);
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
