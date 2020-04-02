using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services
{
    public class VendingMachineService : IVendingMachineService
    {
        private readonly IVendingMachineLoader vendingMachineLoader;
        private readonly IVendingMachineSaver vendingMachineSaver;
        private readonly ICoinValidator coinValidator;
        private VendingMachineState machineState;

        public double CurrentInsertedAmount
        {
            get
            {
                return this.machineState.UserCoinsAccepted.Sum(coin => coin.Key * coin.Value);
            }
        }
        public List<Product> Products
        {
            get
            {
                return this.machineState.ProductStorage.ToList();
            }
        }
        public Dictionary<double, int> CoinsAvailable
        {
            get
            {
                return this.machineState.CoinStorage;
            }
        }

        public VendingMachineService(IVendingMachineLoader vendingMachineLoader, IVendingMachineSaver vendingMachineSaver, ICoinValidator coinValidator)
        {
            this.vendingMachineLoader = vendingMachineLoader;
            this.vendingMachineSaver = vendingMachineSaver;
            this.coinValidator = coinValidator;
            this.machineState = new VendingMachineState();
        }
        /// <summary>
        /// Loads machine data (products, stored coins and coins inserted by the user
        /// </summary>
        public async Task LoadMachine()
        {
            await this.vendingMachineLoader.LoadVendingMachine(this.machineState);
        }
        /// <summary>
        /// Saves machine state data (products, stored coins and coins inserted by the user)
        /// </summary>
        /// <returns></returns>
        public async Task SaveMachine()
        {
            await this.vendingMachineSaver.SaveMachine(this.machineState);
        }
        /// <summary>
        /// Adds a coin to the user's input
        /// </summary>
        /// <param name="denomination"></param>
        public bool AcceptCoin(double denomination)
        {
            if (!this.coinValidator.IsValidCoin(denomination)) return false;
            
            this.AddCoinToBag(this.machineState.UserCoinsAccepted, denomination);
            return true;
        }
        /// <summary>
        /// Returns coins inserted in the machine by the user
        /// </summary>
        /// <returns></returns>
        public IDictionary<double, int> ReturnCoins()
        {
            var returnedCoins = this.machineState.UserCoinsAccepted.ToDictionary(o => o.Key, p => p.Value);
            this.machineState.UserCoinsAccepted.Clear();

            return returnedCoins;
        }
        /// <summary>
        /// Sells a product to the customer.
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Status code for the operation and any change</returns>
        public SellProductResult SellProduct(string productName)
        {
            var product = this.machineState.ProductStorage.FirstOrDefault(prod => prod.Name == productName);

            if (product == null || product.Quantity <= 0) return new SellProductResult() { Status = eSellProductStatus.SoldOut };

            var insertedAmount = this.CurrentInsertedAmount;
            if (insertedAmount < product.Price) return new SellProductResult() { Status = eSellProductStatus.InsufficientFunds };
            product.Quantity -= 1;

            var change = GetChange(product.Price, insertedAmount);

            if (change == null) return new SellProductResult() { Status = eSellProductStatus.OutOfChange };
            this.machineState.UserCoinsAccepted.Clear();


            return new SellProductResult() { Status = eSellProductStatus.Success, Change = change };
        }


        private Dictionary<double, int> GetChange(double price, double insertedAmount)
        {
            
            var availableCoins = this.machineState.UserCoinsAccepted.Select(coin => new Coin() { Denomination = coin.Key, Quantity = coin.Value, IsUser = true })
                .Union(this.machineState.CoinStorage.Select(coin => new Coin() { Denomination = coin.Key, Quantity = coin.Value, IsUser = false }))
                .OrderByDescending(coin => coin.Denomination).ThenByDescending(coin => coin.IsUser).ToList();

            var change = CalculateChange(price, insertedAmount, availableCoins);
            if (change == null) return null;

            StoreUserCoins(availableCoins);

            return change;
        }

        private Dictionary<double, int> CalculateChange(double price, double insertedAmount, List<Coin> availableCoins)
        {
            var change = new Dictionary<double, int>();
            var targetChange = Math.Round(insertedAmount - price, 2);
            var currentChange = change.Sum(coin => coin.Key * coin.Value);
            while (currentChange < targetChange)
            {
                var coinForChange = availableCoins.FirstOrDefault(coin => coin.Denomination <= Math.Round(targetChange - currentChange, 2));
                if (coinForChange == null) return null;
                UpdateAvailableCoins(availableCoins, coinForChange);
                UpdateCoinStorage(coinForChange);
                AddCoinToBag(change, coinForChange.Denomination);
                currentChange = change.Sum(coin => coin.Key * coin.Value);
            }

            return change;
        }

        private void StoreUserCoins(List<Coin> availableCoins)
        {
            var coinsAcceptedFromUser = availableCoins.Where(coin => coin.IsUser);
            foreach (var coin in coinsAcceptedFromUser)
            {
                this.AddCoinToBag(this.machineState.CoinStorage, coin.Denomination);
            }
        }

        private void UpdateCoinStorage(Coin coinForChange)
        {
            if (!coinForChange.IsUser)
            {
                this.machineState.CoinStorage[coinForChange.Denomination] -= 1;
                if (this.machineState.CoinStorage[coinForChange.Denomination] == 0) this.machineState.CoinStorage.Remove(coinForChange.Denomination);
            }            
        }

        private void UpdateAvailableCoins(List<Coin> availableCoins, Coin coinForChange)
        {
            if (coinForChange.Quantity == 1) availableCoins.Remove(coinForChange);
            if (coinForChange.Quantity > 1) coinForChange.Quantity -= 1;            
        }

        private void AddCoinToBag(Dictionary<double, int> coinBag, double denomination)
        {
            if (!coinBag.ContainsKey(denomination))
            {
                coinBag.Add(denomination, 0);
            }
            coinBag[denomination] += 1;
        }

        /// <summary>
        /// Adds a coin directly to storage, to be used for the initial load of the machine
        /// </summary>
        /// <param name="coinDenomination"></param>
        public void AddCoin(double coinDenomination)
        {
            this.AddCoinToBag(this.machineState.CoinStorage, coinDenomination);
        }
        /// <summary>
        /// Adds a product to storage, to be used for the initial load of the machine
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            this.machineState.ProductStorage.Add(product);
        }

        private class Coin
        {
            public double Denomination { get; set; }
            public int Quantity { get; set; }
            public bool IsUser { get; set; }
        }
    }
}
