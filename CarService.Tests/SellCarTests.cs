using CarService.BL.Interfaces;
using CarService.BL.Services;
using CarService.DL.Interfaces;
using CarService.Models.Entities;
using Moq;

namespace CarService.Tests
{
    public class SellCarTests
    {

        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly Mock<ICarRepository> _carRepositoryMock;

        public SellCarTests()
        {
            _carRepositoryMock = new Mock<ICarRepository>();
            _customerServiceMock = new Mock<ICustomerService>();    
        }

        [Fact]
        public void SellCar_ApplyDiscount_Ok()
        {
            var carId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            _carRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(() => new Car
            {
                BasePrice = 20000,
                Id = carId,
                Model = "Model S",
                Year = 2020
            });

            _customerServiceMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(() => new Customer
            {
                Id = carId,
                Discount = 1500,


            });

            var sellCarService = new SellCarService(
                _customerServiceMock.Object,
                _carRepositoryMock.Object);

            var result = sellCarService.SellCar(customerId, carId);

            Assert.NotNull(result);
            Assert.Equal(18500, result.SalePrice);
        }

        [Fact]
        public void SellCar_ApplyDiscount_MissingCustomer()
        {
            var carId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            _carRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(() => null);

            _customerServiceMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(() => new Customer
            {
                Id = carId,
                Discount = 1500,
            });

            var sellCarService = new SellCarService(
                _customerServiceMock.Object,
                _carRepositoryMock.Object);

            var result = sellCarService.SellCar(customerId, carId);

            Assert.Null(result);
        }

        [Fact]
        public void SellCar_ApplyDiscount_MissingCar()
        {
            var carId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            _carRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(() => new Car
            {
                BasePrice = 20000,
                Id = carId,
                Model = "Model S",
                Year = 2020
            });

            _customerServiceMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(() => null);

            var sellCarService = new SellCarService(
                _customerServiceMock.Object,
                _carRepositoryMock.Object);

            var result = sellCarService.SellCar(customerId, carId);

            Assert.Null(result);
        }

        [Fact]
        public void SellCar_ApplyNegativeDiscount()
        {
            var carId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            _carRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(() => new Car
            {
                BasePrice = 20000,
                Id = carId,
                Model = "Model S",
                Year = 2020
            });

            _customerServiceMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(() => new Customer
            {
                Id = carId,
                Discount = 150000,
            });

            var sellCarService = new SellCarService(
                _customerServiceMock.Object,
                _carRepositoryMock.Object);

            var result = sellCarService.SellCar(customerId, carId);

            Assert.NotNull(result);
            Assert.Equal(0, result.SalePrice);

        }
    }
}
