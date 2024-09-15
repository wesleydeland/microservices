using System;
using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;

namespace CommandService.Profiles;

public class CommandProfile : Profile
{
    public CommandProfile()
    {
        //source -> target
        CreateMap<Platform, PlatformReadDTO>();
        CreateMap<CommandCreateDTO, Command>();
        CreateMap<Command, CommandReadDTO>();

        CreateMap<PlatformPublishDTO, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
    }
}
