using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace PlatformService.Repositories.Car
{
    public interface ICarRepository
    {
        // Create
        Task<ObjectId> Create(Models.Car car);

        // Read
        Task<Models.Car> Get(ObjectId objectId);
        Task<IEnumerable<Models.Car>> Get();
        Task<IEnumerable<Models.Car>> GetByMake(string make);

        // Update
        Task<bool> Update(ObjectId objectId, Models.Car car);

        // Delete
        Task<bool> Delete(ObjectId objectId);
    }
}