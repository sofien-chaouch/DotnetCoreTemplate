using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using PlatformService.Services.Platform;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformService _platformService;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformService platformService, IMapper mapper)
        {
            _platformService = platformService;
            _mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            var platforms = _platformService.GetAllPlatforms();

            foreach(var plat in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }

            return Task.FromResult(response);
        }
    }
}