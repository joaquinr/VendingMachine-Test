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
        public List<Coin> StorageCoins { get; set; }
        public List<Product> StorageProducts { get; set; }
        public List<Coin> UserAcceptedCoins { get; set; }

        public TestMachineLoader()
        {
            this.StorageCoins = new List<Coin>();
            this.StorageProducts = new List<Product>();
            this.UserAcceptedCoins = new List<Coin>();
        }
        public Task LoadVendingMachine(VendingMachineState machine)
        {
            this.StorageCoins.ForEach(coin => machine.CoinStorage.Add(coin));
            this.UserAcceptedCoins.ForEach(coin => machine.UserCoinsAccepted.Add(coin));
            this.StorageProducts.ForEach(product => machine.ProductStorage.Add(product));

            return Task.FromResult(0);
        }
    }
}
