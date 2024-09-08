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
    }
}
