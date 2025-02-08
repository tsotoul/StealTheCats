using AutoMapper;
using StealTheCats.Models;
using StealTheCats.Repositories.Interfaces;
using StealTheCats.Services.Interfaces;

namespace StealTheCats.Services
{
    public class CatsService : ICatsService
    {
        private readonly ICatsApiRepository _catsApiRepository;
        private readonly IMapper _mapper;

        public CatsService(ICatsApiRepository catsApiRepository, IMapper mapper)
        {
            _catsApiRepository = catsApiRepository;
            _mapper = mapper;
        }

        public async Task FetchCats()
        {
            // Fetch the cats as CatDtos
            var catsDtos = await _catsApiRepository.GetCatsAsync();

            // Map the CatDtos to Cat
            var catsFromTheApi = _mapper.Map<List<Cat>>(catsDtos);


            // Save Cats to the DB
            throw new NotImplementedException();
        }
    }
}
