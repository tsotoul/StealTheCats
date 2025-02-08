using AutoMapper;
using StealTheCats.Dtos;
using StealTheCats.Models;

namespace StealTheCats.Mappings
{
    public class MapDomainToProfileDto : Profile
    {
        public MapDomainToProfileDto()
        {
            CreateMap<Cat, DatabaseCatDto>();
        }
    }
}
