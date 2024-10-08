using System;
using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Profiles;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        //Source -> target
        CreateMap<Platform, PlatformReadDTO>();

        CreateMap<PlatformCreateDTO, Platform>();

        CreateMap<PlatformReadDTO, PlatformPublishDTO>();

        CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}
