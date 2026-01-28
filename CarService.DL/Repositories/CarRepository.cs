using CarService.DL.Interfaces;
using CarService.Models.Configurations;
using CarService.Models.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CarService.DL.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IMongoCollection<Car> _carsCollection;
        private readonly IOptionsMonitor<MongoDbConfiguration> _mongoDbConfiguration;

        private readonly ILogger<CarRepository> _logger;

        public CarRepository(
            IOptionsMonitor<MongoDbConfiguration> mongoDbConfiguration,
            ILogger<CarRepository> logger)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
            _logger = logger;

            var client = new MongoClient(_mongoDbConfiguration.CurrentValue.ConnectionString);
            var database = client.GetDatabase(_mongoDbConfiguration.CurrentValue.DatabaseName);

            _carsCollection = database.GetCollection<Car>($"{nameof(Car)}s");
        }

        public void Add(Car? car)
        {
            if (car == null) return;

            try
            {
                _carsCollection.InsertOne(car);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Add)}: {e.Message} - {e.StackTrace}");
            }
        }

        public List<Car> GetAll()
        {
            return _carsCollection.Find(_ => true).ToList();
        }

        public Car GetById(Guid id)
        {
            if (id == Guid.Empty) return default;

            try
            {
                return _carsCollection.Find(c => c.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in method {nameof(GetById)}: {e.Message} - {e.StackTrace}");
            }

            return default;
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty) return;

            try
            {
                var result = _carsCollection.DeleteOne(c => c.Id == id);

                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning($"No car found with Id: {id} to delete.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in method {nameof(Delete)}: {e.Message} - {e.StackTrace}");
            }
        }
    }
}