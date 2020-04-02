using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services
{
    public interface IVendingMachineService
    {
        Dictionary<double, int> CoinsAvailable { get; }
        double CurrentInsertedAmount { get; }
        List<Product> Products { get; }

        bool AcceptCoin(double denomination);
        void AddCoin(double coinDenomination);
        void AddProduct(Product product);
        Task LoadMachine();
        IDictionary<double, int> ReturnCoins();
        Task SaveMachine();
        SellProductResult SellProduct(string productName);
    }
}