using System.Collections.Generic;
using PlatformService.Interfaces;
using PlatformService.Repositories.Platform;

namespace PlatformService.Services.Platform{


    public class PlatformService : IPlatformService{
        private readonly IPlatformRepository _platformRepository;
        public PlatformService( IPlatformRepository platformRepository){
            _platformRepository = platformRepository;
        }


        public List<Models.Platform> GetAllPlatforms()
        {
            return _platformRepository.GetAllPlatforms();
        }
        public Models.Platform AddPlatform(Models.Platform platform)
        {
            return _platformRepository.AddPlatform(platform);
        }
        public Models.Platform GetPlatformById(int id)
        {
            return _platformRepository.GetPlatformById(id);
        }
        public Models.Platform UpdatePlatform(Models.Platform platform)
        {
            return _platformRepository.UpdatePlatform(platform);
        }
        public void DeletePlatform(Models.Platform platform)
        { 
            _platformRepository.Delete(platform);
        }
    }
}
