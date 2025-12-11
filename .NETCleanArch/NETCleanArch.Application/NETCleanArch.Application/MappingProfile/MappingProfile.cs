using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCleanArchApplication.MappingProfile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<NETCleanArchDomain.Entities.ApplicationMaster, NETCleanArchApplication.Dtos.ApplicationDto>()
                .ForMember(dest => dest.ServiceIds, opt => opt.Ignore());
            CreateMap<NETCleanArchApplication.Dtos.ApplicationDto, NETCleanArchDomain.Entities.ApplicationMaster>()
                .ForMember(dest => dest.Services, opt => opt.Ignore());
            CreateMap<NETCleanArchDomain.Entities.ApplicationServiceMapping, NETCleanArchApplication.Dtos.ApplicationDto>()
                .ForMember(dest => dest.ServiceIds, opt => opt.Ignore());
            CreateMap<NETCleanArchApplication.Dtos.ServiceRegistryDto, NETCleanArchDomain.Entities.ApplicationServiceMapping>()
                .ForMember(dest => dest.AppServiceMappingId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());
            CreateMap<NETCleanArchDomain.Entities.ServiceSchema, NETCleanArchApplication.Dtos.ServiceSchemaDto>();
            CreateMap<NETCleanArchApplication.Dtos.ServiceSchemaDto, NETCleanArchDomain.Entities.ServiceSchema>()
                .ForMember(dest => dest.SchemaId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());
            //CreateMap<NETCleanArchDomain.Entities.ServiceMaster, NETCleanArchApplication.Dtos.ServiceMasterDto>();
            //CreateMap<NETCleanArchApplication.Dtos.ServiceMasterDto, NETCleanArchDomain.Entities.ServiceMaster>()
            //    .ForMember(dest => dest.ServiceId, opt => opt.Ignore())
            //    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            //    .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            //    .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            //    .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());

        }
    }
}
