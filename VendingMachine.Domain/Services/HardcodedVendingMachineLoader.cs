using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services
{
    //Should replace with a database or config file data source, but for the requirements of this project, hardcoded is fine
    public class HardcodedVendingMachineLoader : IVendingMachineLoader
    {
        public Task LoadVendingMachine(VendingMachineState machine)
        {
            LoadProduct(machine, productName: "Tea", productPrice: 1.30, quantity: 10);
            LoadProduct(machine, productName: "Espresso", productPrice: 1.80, quantity: 20);
            LoadProduct(machine, productName: "Juice", productPrice: 1.80, quantity: 20);
            LoadProduct(machine, productName: "Chicken soup", productPrice: 1.80, quantity: 15);

            LoadCoin(machine, quantity: 100, coinDenomination: 0.1);
            LoadCoin(machine, quantity: 100, coinDenomination: 0.2);
            LoadCoin(machine, quantity: 100, coinDenomination: 0.5);
            LoadCoin(machine, quantity: 100, coinDenomination: 1);

            return Task.FromResult(0);
        }

        private static void LoadCoin(VendingMachineState machine, int quantity, double coinDenomination)
        {
            for (int i = 0; i < quantity; i++)
            {
                machine.CoinStorage.Add(new Coin() { Denomination = coinDenomination });
            }
        }

        private static void LoadProduct(VendingMachineState machine, string productName, double productPrice, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                machine.ProductStorage.Add(new Product() { Name = productName, Price = productPrice });
            }
        }
    }
}
