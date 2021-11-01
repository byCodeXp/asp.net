using AutoMapper;
using Data.Entities;
using Models.Entities;
using Models.Requests;

namespace Api
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterRequest, User>().ReverseMap();
            CreateMap<User, UserVM>().ReverseMap();
        }
    }
}