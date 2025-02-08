using AutoMapper;
using StealTheCatsApi.Dtos;
using StealTheCatsApi.Models;

namespace StealTheCatsApi.Mappings
{
    public class MapDomainToProfileDto : Profile
    {
        public MapDomainToProfileDto()
        {
            CreateMap<Cat, DatabaseCatDto>();
        }
    }
}
