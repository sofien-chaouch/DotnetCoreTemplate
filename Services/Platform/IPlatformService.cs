using System.Collections.Generic;

namespace PlatformService.Services.Platform{
    public interface IPlatformService{

        public List<Models.Platform> GetAllPlatforms();

        public Models.Platform AddPlatform(Models.Platform platform);
        public Models.Platform GetPlatformById(int id);
        public Models.Platform UpdatePlatform(Models.Platform platform);
        
        public void DeletePlatform(Models.Platform platform);

    }
}
