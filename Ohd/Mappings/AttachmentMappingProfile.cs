using AutoMapper;
using Ohd.DTOs.Requests;
using Ohd.Entities;

namespace Ohd.Mappings
{
    public class AttachmentMappingProfile : Profile
    {
        public AttachmentMappingProfile()
        {
            CreateMap<AttachmentCreateDto, Attachment>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}