using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VendingMachine.Domain
{
    public class VendingMachine
    {
        private readonly IVendingMachineLoader vendingMachineLoader;
        private List<Coin> coinStorage;
        private List<Product> productStorage;
        private List<Coin> userCoinsAccepted;

        public double CurrentInsertedAmount
        {
            get
            {
                return this.userCoinsAccepted.Sum(coin => coin.Denomination);
            }
        }

        public VendingMachine(IVendingMachineLoader vendingMachineLoader)
        {
            this.vendingMachineLoader = vendingMachineLoader;
            this.coinStorage = new List<Coin>();
            this.productStorage = new List<Product>();
            this.userCoinsAccepted = new List<Coin>();
        }
        public void LoadMachine()
        {
            this.vendingMachineLoader.LoadVendingMachine(this);
        }
        public void AcceptCoin(Coin coin)
        {
            this.userCoinsAccepted.Add(coin);
        }
        public IEnumerable<Coin> ReturnCoins()
        {
            throw new NotImplementedException();
        }
        public void SellProduct(string product)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a coin directly to storage, to be used for the initial load of the machine
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="coinDenomination"></param>
        public void AddCoin(double coinDenomination)
        {
            this.coinStorage.Add(new Coin() { Denomination = coinDenomination });
        }
        /// <summary>
        /// Adds a product to storage, to be used for the initial load of the machine
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            this.productStorage.Add(product);
        }
    }
}
