using CarService.Models.Entities;

namespace CarService.DL.Interfaces
{
    public interface ICarRepository
    {

        void Add(Car? customer);
        List<Car> GetAll();
        Car? GetById(Guid id);
        void Delete(Guid id);
    }
}
