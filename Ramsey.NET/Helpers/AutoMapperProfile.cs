using AutoMapper;
using Ramsey.NET.Dtos;
using Ramsey.NET.Models;

namespace WebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AdminUser, AdminDto>();
            CreateMap<AdminDto, AdminUser>();
        }
    }
}