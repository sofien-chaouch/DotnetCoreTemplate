using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Data.Redis
{
    public interface IRedisPlatformRepo
    {
        void CreatePlatform(Platform plat);
        Platform? GetPlatformById(string id);
        IEnumerable<Platform?>? GetAllPlatforms();
    }
}