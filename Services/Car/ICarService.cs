using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace PlatformService.Services.Car
{
    public interface ICarService
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