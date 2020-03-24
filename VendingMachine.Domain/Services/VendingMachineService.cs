using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services
{
    public class VendingMachineService
    {
        private readonly IVendingMachineLoader vendingMachineLoader;
        private readonly IVendingMachineSaver vendingMachineSaver;
        private VendingMachineState machineState;

        public double CurrentInsertedAmount
        {
            get
            {
                return this.machineState.UserCoinsAccepted.Sum(coin => coin.Denomination);
            }
        }
        public List<Product> Products 
        { 
            get
            {
                return this.machineState.ProductStorage.ToList();
            }
        }
        public List<Coin> CoinsAvailable
        {
            get
            {
                return this.machineState.CoinStorage.ToList();
            }
        }

        public VendingMachineService(IVendingMachineLoader vendingMachineLoader, IVendingMachineSaver vendingMachineSaver)
        {
            this.vendingMachineLoader = vendingMachineLoader;
            this.vendingMachineSaver = vendingMachineSaver;
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
        /// <param name="coin"></param>
        public void AcceptCoin(Coin coin)
        {
            this.machineState.UserCoinsAccepted.Add(coin);
        }
        /// <summary>
        /// Returns coins inserted in the machine by the user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Coin> ReturnCoins()
        {
            var returnedCoins = this.machineState.UserCoinsAccepted.ToList();
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

            if (product == null) return new SellProductResult() { Status = eSellProductStatus.SoldOut };

            var insertedAmount = this.machineState.UserCoinsAccepted.Sum(coin => coin.Denomination);
            if (insertedAmount < product.Price) return new SellProductResult() { Status = eSellProductStatus.InsufficientFunds };
            this.machineState.ProductStorage.Remove(product);

            var change = GetChange(product.Price, insertedAmount);

            if (change == null) return new SellProductResult() { Status = eSellProductStatus.OutOfChange };
            RemoveUsedCoinsFromStorage(change);

            return new SellProductResult() { Status = eSellProductStatus.Success, Change = change };
        }

        private void RemoveUsedCoinsFromStorage(List<Coin> change)
        {
            foreach (var coin in change)
            {
                if (this.machineState.UserCoinsAccepted.Contains(coin))
                    this.machineState.UserCoinsAccepted.Remove(coin);
                else
                    this.machineState.CoinStorage.Remove(coin);
            }
        }

        private List<Coin> GetChange(double price, double insertedAmount)
        {
            var availableCoins = this.machineState.CoinStorage.Union(this.machineState.UserCoinsAccepted).OrderByDescending(coin => coin.Denomination).ToList();
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
            this.machineState.CoinStorage.Add(new Coin() { Denomination = coinDenomination });
        }
        /// <summary>
        /// Adds a product to storage, to be used for the initial load of the machine
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            this.machineState.ProductStorage.Add(product);
        }
    }
}
