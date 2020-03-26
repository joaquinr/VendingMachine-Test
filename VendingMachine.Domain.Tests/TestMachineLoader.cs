using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services;

namespace VendingMachine.Domain.Tests
{
    public class TestMachineLoader : IVendingMachineLoader
    {
        public Dictionary<double, int> StorageCoins { get; set; }
        public List<Product> StorageProducts { get; set; }
        public Dictionary<double, int> UserAcceptedCoins { get; set; }

        public TestMachineLoader()
        {
            this.StorageCoins = new Dictionary<double, int>();
            this.StorageProducts = new List<Product>();
            this.UserAcceptedCoins = new Dictionary<double, int>();
        }
        public Task LoadVendingMachine(VendingMachineState machine)
        {
            foreach (var coin in this.StorageCoins.Keys)
            {
                machine.CoinStorage.Add(coin, this.StorageCoins[coin]);
            }
            foreach (var coin in this.UserAcceptedCoins.Keys)
            {
                machine.UserCoinsAccepted.Add(coin, this.UserAcceptedCoins[coin]);
            }
            this.StorageProducts.ForEach(product => machine.ProductStorage.Add(product));

            return Task.FromResult(0);
        }
    }
}
