using System.Collections.Generic;
using VendingMachine.Domain.Models;

namespace VendingMachine.Api
{
    public class VendingMachineData
    {
        public double CurrentInsertedAmount { get; internal set; }
        public List<Product> AvailableProducts { get; internal set; }
    }
    public class ProductData
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
