
using System.Collections.Generic;

namespace PlatformService.Repositories.Platform
{
    public interface IPlatformRepository : IRepository<Models.Platform>
    {
        public Models.Platform AddPlatform(Models.Platform platform);
        public Models.Platform GetPlatformById(int id);
        public Models.Platform UpdatePlatform(Models.Platform platform);
        public List<Models.Platform> GetAllPlatforms();

        public void DeletePlatform(Models.Platform platform);

    }
}