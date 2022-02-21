using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace PlatformService.Repositories.Car
{

    public class CarRepository : ICarRepository
    {
        private readonly IMongoCollection<Models.Car> _cars;

        public CarRepository(IMongoClient client)
        {
            var database = client.GetDatabase("MyDb");
            var collection = database.GetCollection<Models.Car>(nameof(Car));

            _cars = collection;
        }

        public async Task<ObjectId> Create(Models.Car car)
        {
            await _cars.InsertOneAsync(car);

            return car.Id;
        }

        public async Task<bool> Delete(ObjectId objectId)
        {
            var filter = Builders<Models.Car>.Filter.Eq(c => c.Id, objectId);
            var result = await _cars.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }

        public Task<Models.Car> Get(ObjectId objectId)
        {
            var filter = Builders<Models.Car>.Filter.Eq(c => c.Id, objectId);
            var car = _cars.Find(filter).FirstOrDefaultAsync();

            return car;
        }

        public async Task<IEnumerable<Models.Car>> Get()
        {
            var cars = await _cars.Find(_ => true).ToListAsync();

            return cars;
        }

        public async Task<IEnumerable<Models.Car>> GetByMake(string make)
        {
            var filter = Builders<Models.Car>.Filter.Eq(c => c.Make, make);
            var cars = await _cars.Find(filter).ToListAsync();

            return cars;
        }

        public async Task<bool> Update(ObjectId objectId, Models.Car car)
        {
            var filter = Builders<Models.Car>.Filter.Eq(c => c.Id, objectId);
            var update = Builders<Models.Car>.Update
                .Set(c => c.Make, car.Make)
                .Set(c => c.Model, car.Model)
                .Set(c => c.TopSpeed, car.TopSpeed);
            var result = await _cars.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }
    }
}