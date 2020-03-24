using System;
using System.Collections.Generic;
using System.Text;

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
        public void LoadVendingMachine(VendingMachine machine)
        {
            this.StorageCoins.ForEach(coin => machine.AddCoin(coin.Denomination));
            this.UserAcceptedCoins.ForEach(coin => machine.AcceptCoin(coin));
            this.StorageProducts.ForEach(product => machine.AddProduct(product));
        }
    }
}
