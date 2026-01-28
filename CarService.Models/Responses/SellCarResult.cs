using CarService.Models.Entities;

namespace CarService.Models.Responses
{
    public class SellCarResult
    {
        public Customer Customer { get; set; }

        public Car Car { get; set; }

        public decimal SalePrice { get; set; }

    }
}
