using CarService.BL.Interfaces;
using CarService.DL.Interfaces;
using CarService.Models.Responses;

namespace CarService.BL.Services
{
    public class SellCarService : ISellCarService
    {
        private readonly ICustomerService _customerService;
        private readonly ICarRepository _carRepository;

        public SellCarService(ICustomerService customerService, ICarRepository carRepository)
        {
            _customerService = customerService;
            _carRepository = carRepository;

        }

        public SellCarResult? SellCar(Guid customerId, Guid carId)
        {
            var customer = _customerService.GetById(customerId);
            var car = _carRepository.GetById(carId);

            if (customer == null || car == null)
            {
                return null;
            }

            var price = car.BasePrice - customer.Discount;

            return new SellCarResult
            {
                Customer = customer,
                Car = car,
                SalePrice = price < 0 ? 0 : price

            };
        }
    }
}
