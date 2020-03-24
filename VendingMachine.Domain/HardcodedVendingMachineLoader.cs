using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Domain
{
    //Should replace with a database or config file data source, but for the requirements of this project, hardcoded is fine
    public class HardcodedVendingMachineLoader : IVendingMachineLoader
    {
        public void LoadVendingMachine(VendingMachine machine)
        {
            LoadProduct(machine, productName: "Tea", productPrice: 1.30, quantity: 10);
            LoadProduct(machine, productName: "Espresso", productPrice: 1.80, quantity: 20);
            LoadProduct(machine, productName: "Juice", productPrice: 1.80, quantity: 20);
            LoadProduct(machine, productName: "Chicken soup", productPrice: 1.80, quantity: 15);

            LoadCoin(machine, quantity: 100, coinDenomination: 0.1);
            LoadCoin(machine, quantity: 100, coinDenomination: 0.2);
            LoadCoin(machine, quantity: 100, coinDenomination: 0.5);
            LoadCoin(machine, quantity: 100, coinDenomination: 1);
        }

        private static void LoadCoin(VendingMachine machine, int quantity, double coinDenomination)
        {
            for (int i = 0; i < quantity; i++)
            {
                machine.AddCoin(coinDenomination);
            }
        }

        private static void LoadProduct(VendingMachine machine, string productName, double productPrice, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                machine.AddProduct(new Product() { Name = productName, Price = productPrice });
            }
        }
    }
}
