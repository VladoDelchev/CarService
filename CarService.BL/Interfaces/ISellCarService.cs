using CarService.Models.Responses;

namespace CarService.BL.Interfaces
{
    public interface ISellCarService
    {
        SellCarResult? SellCar(Guid customerId, Guid carId);
    }
}
