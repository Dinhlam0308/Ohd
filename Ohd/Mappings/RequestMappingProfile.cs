using AutoMapper;
using Ohd.DTOs.Requests;
using Ohd.Entities;

namespace Ohd.Mappings
{
    public class RequestMappingProfile : Profile
    {
        public RequestMappingProfile()
        {
            // Create
            CreateMap<RequestCreateDto, Request>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Update
            CreateMap<RequestUpdateDto, Request>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Change status không cần map toàn bộ
        }
    }
}