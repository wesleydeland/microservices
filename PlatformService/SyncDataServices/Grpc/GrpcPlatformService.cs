using System;
using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using System.Linq;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepository platformRepository, IMapper mapper)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
    }

    public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var response = new PlatformResponse();
        var platforms = await _platformRepository.GetAllPlatforms();

        foreach (var platform in platforms)
        {
            response.Platforms.Add(_mapper.Map<GrpcPlatformModel>(platform));
        }

        return response;
    }
}
