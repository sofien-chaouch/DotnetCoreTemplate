using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using PlatformService.Repositories.Car;

namespace PlatformService.Services.Car
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        public CarService( ICarRepository carRepository){
            _carRepository = carRepository;
        }
        public Task<ObjectId> Create(Models.Car car)
        {
            return _carRepository.Create(car);
        }
        public Task<Models.Car> Get(ObjectId objectId)
        {
            return _carRepository.Get(objectId);

        }
        public Task<IEnumerable<Models.Car>> Get()
        {
            return _carRepository.Get();
        }
        public Task<IEnumerable<Models.Car>> GetByMake(string make)
        {
            return _carRepository.GetByMake(make);
        }
        public Task<bool> Update(ObjectId objectId, Models.Car car)
        {
            return Update(objectId,car);
        }
        public Task<bool> Delete(ObjectId objectId)
        {
            return Delete(objectId);
        }
    }
}