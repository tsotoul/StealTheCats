using AutoMapper;
using StealTheCatsApi.Dtos;
using StealTheCatsApi.Models;

namespace StealTheCatsApi.Mappings
{
    public class MapProfileDtoToDomain : Profile
    {
        public MapProfileDtoToDomain()
        {
            CreateMap<ApiCatDto, Cat>()
                .ForMember(dest => dest.CatId, src => src.MapFrom(m => m.Id))
                .ForMember(dest => dest.Image, src => src.MapFrom(m => m.Url))
                .ForMember(dest => dest.Temperaments, src => src.MapFrom(m => ExtractTemperaments(m.Breeds)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }

        private List<string> ExtractTemperaments(IEnumerable<BreedDto> breeds)
        {
            return breeds
                .SelectMany(breed => breed.Temperament.Split(',').Select(t => t.Trim()).Select(t => t.ToUpper())).ToList();
        }
    }
}
