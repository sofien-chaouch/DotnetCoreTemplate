using System.Collections.Generic;
using System.Linq;
using PlatformService.Data;
using PlatformService.Models;
using PlatformService.Repositories.Platform;
using PlatformService.Repositories.RepositoriesPatterns;

namespace PlatformService.Repositories.Platform
{
    public class PlatformRepository : Repository<Models.Platform> , IPlatformRepository
    {
        private IPlatformRepository _platformRepositoryImplementation;
        public PlatformRepository(AppDbContext context) : base(context)
        {
            
        }


        public Models.Platform AddPlatform(Models.Platform platform)
        {
            return AddAsync(platform).Result;
        }

        public Models.Platform GetPlatformById(int id)
        {
            return  GetAll().FirstOrDefault(p=>p.Id==id);
        }

        public Models.Platform UpdatePlatform(Models.Platform platform)
        {
            return Update(platform);
        }
        public List<Models.Platform> GetAllPlatforms()
        {
            return GetAll().ToList();
        }
        public void DeletePlatform(Models.Platform platform)
        {
            Delete(platform);
        }
    }
}