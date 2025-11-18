using AutoMapper;
using Ohd.DTOs.Requests;
using Ohd.Entities;

namespace Ohd.Mappings
{
    public class RequestCommentMappingProfile : Profile
    {
        public RequestCommentMappingProfile()
        {
            CreateMap<RequestCommentCreateDto, Request_Comments>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.AttachmentsCount, opt => opt.MapFrom(_ => 0));
        }
    }
}