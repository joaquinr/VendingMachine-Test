using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var returnedCoins = this.userCoinsAccepted.ToList();
            this.userCoinsAccepted.Clear();

            return returnedCoins;
        }
        public SellProductResult SellProduct(string productName)
        {
            var product = this.productStorage.FirstOrDefault(prod => prod.Name == productName);

            if (product == null) return new SellProductResult() { Status = eSellProductStatus.SoldOut };
            
            var insertedAmount = this.userCoinsAccepted.Sum(coin => coin.Denomination);
            if (insertedAmount < product.Price) return new SellProductResult() { Status = eSellProductStatus.InsufficientFunds };
            this.productStorage.Remove(product);
            
            var change = GetChange(product.Price, insertedAmount);
            
            if(change == null) return new SellProductResult() { Status = eSellProductStatus.OutOfChange };

            foreach(var coin in change) {
                if (this.userCoinsAccepted.Contains(coin))
                    this.userCoinsAccepted.Remove(coin);
                else
                    this.coinStorage.Remove(coin);                
            }

            return new SellProductResult() { Status = eSellProductStatus.Success, Change = change };
        }
        private List<Coin> GetChange(double price, double insertedAmount)
        {
            var availableCoins = this.coinStorage.Union(this.userCoinsAccepted).OrderByDescending(coin => coin.Denomination).ToList();
            var change = new List<Coin>();

            var targetChange = insertedAmount - price;
            while (change.Sum(coin => coin.Denomination) < targetChange)
            {
                var coinForChange = availableCoins.FirstOrDefault(coin => coin.Denomination <= targetChange);
                if (coinForChange == null) return null;

                availableCoins.Remove(coinForChange);
                change.Add(coinForChange);
            }

            return change;
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
