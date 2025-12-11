using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCleanArch.Application.MappingProfile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<NETCleanArchDomain.Entities.ApplicationMaster, NETCleanArch.Application.Dtos.ApplicationDto>()
                .ForMember(dest => dest.ServiceIds, opt => opt.Ignore());
            CreateMap<NETCleanArch.Application.Dtos.ApplicationDto, NETCleanArchDomain.Entities.ApplicationMaster>()
                .ForMember(dest => dest.Services, opt => opt.Ignore());
            CreateMap<NETCleanArchDomain.Entities.ApplicationServiceMapping, NETCleanArch.Application.Dtos.ApplicationDto>()
                .ForMember(dest => dest.ServiceIds, opt => opt.Ignore());
            CreateMap<NETCleanArch.Application.Dtos.ServiceRegistryDto, NETCleanArchDomain.Entities.ApplicationServiceMapping>()
                .ForMember(dest => dest.AppServiceMappingId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());
            CreateMap<NETCleanArchDomain.Entities.ServiceSchema, NETCleanArch.Application.Dtos.ServiceSchemaDto>();
            CreateMap<NETCleanArch.Application.Dtos.ServiceSchemaDto, NETCleanArchDomain.Entities.ServiceSchema>()
                .ForMember(dest => dest.SchemaId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());
            //CreateMap<NETCleanArchDomain.Entities.ServiceMaster, NETCleanArch.Application.Dtos.ServiceMasterDto>();
            //CreateMap<NETCleanArch.Application.Dtos.ServiceMasterDto, NETCleanArchDomain.Entities.ServiceMaster>()
            //    .ForMember(dest => dest.ServiceId, opt => opt.Ignore())
            //    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            //    .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            //    .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            //    .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());

        }
    }
}
