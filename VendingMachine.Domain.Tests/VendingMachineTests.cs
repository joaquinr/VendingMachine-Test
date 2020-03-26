using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;
using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services;

namespace VendingMachine.Domain.Tests
{
    [TestClass]
    public class VendingMachineTests
    {
        [TestMethod]
        public void When_User_Inserts_A_Coin_Vending_Machine_Stores_It()
        {
            var machine = new VendingMachineService(null, null);

            machine.AcceptCoin(0.5);

            machine.CurrentInsertedAmount.ShouldBe(0.5);
        }
        [TestMethod]
        public void When_User_Asks_To_Return_Coins_Coins_Are_Returned()
        {
            var machine = new VendingMachineService(null, null);

            machine.AcceptCoin(0.5);
            var returnedCoins = machine.ReturnCoins();

            machine.CurrentInsertedAmount.ShouldBe(0);
            returnedCoins.Sum(coin => coin.Key * coin.Value).ShouldBe(0.5);
        }
        [TestMethod]
        public void When_User_Inserts_Just_Enough_Coins_And_Purchases_A_Product_Success_Is_Returned()
        {
            var productName = "Test";
            var testProduct = new Product() { Name = productName, Price = 1, Quantity = 1 };
            var machine = new VendingMachineService(null, null);
            machine.AddProduct(testProduct);

            machine.AcceptCoin(1);
            var result = machine.SellProduct(productName);

            result.Status.ShouldBe(eSellProductStatus.Success);
            result.Change.Any().ShouldBeFalse();
        }
        [TestMethod]
        public void When_User_Inserts_More_Than_Enough_Coins_And_Purchases_A_Product_Success_Message_Is_Returned_And_Change_Is_Returned()
        {
            var productName = "Test";
            var testProduct = new Product() { Name = productName, Price = 5, Quantity = 1 };
            var machineLoader = new TestMachineLoader();
            machineLoader.StorageProducts.Add(testProduct);
            machineLoader.StorageCoins.Add(1, 1);
            machineLoader.StorageCoins.Add(2, 1);
            machineLoader.StorageCoins.Add(3, 1);

            var machine = new VendingMachineService(machineLoader, null);
            machine.LoadMachine().Wait();

            machine.AcceptCoin(2);
            machine.AcceptCoin(2);
            machine.AcceptCoin(2);

            var result = machine.SellProduct(productName);

            result.Status.ShouldBe(eSellProductStatus.Success);
            result.Change.Sum(coin => coin.Key * coin.Value).ShouldBe(1);
        }
        [TestMethod]
        public void When_User_Inserts_More_Than_Enough_Coins_And_Purchases_A_Product_Success_Message_Is_Returned_And_Change_Is_Returned_With_Highest_Denomination_Coins()
        {
            var productName = "Test";
            var testProduct = new Product() { Name = productName, Price = 5, Quantity = 1 };
            var machineLoader = new TestMachineLoader();
            machineLoader.StorageProducts.Add(testProduct);
            machineLoader.StorageCoins.Add(1, 2);
            machineLoader.StorageCoins.Add(2, 1);

            var machine = new VendingMachineService(machineLoader, null);
            machine.LoadMachine().Wait();

            machine.AcceptCoin(1);
            machine.AcceptCoin(2);
            machine.AcceptCoin(4);

            var result = machine.SellProduct(productName);

            result.Status.ShouldBe(eSellProductStatus.Success);
            result.Change.Sum(coin => coin.Key * coin.Value).ShouldBe(2);
        }
        [TestMethod]
        public void When_User_Inserts_More_Than_Enough_Coins_And_Purchases_A_Product_Success_Message_Is_Returned_And_Change_Is_Returned_With_Highest_Denomination_Coins_Including_Users()
        {
            var productName = "Test";
            var testProduct = new Product() { Name = productName, Price = 5, Quantity = 1 };
            var machineLoader = new TestMachineLoader();
            machineLoader.StorageProducts.Add(testProduct);
            machineLoader.StorageCoins.Add(1, 3);
            
            var machine = new VendingMachineService(machineLoader, null);
            machine.LoadMachine().Wait();

            machine.AcceptCoin(1);
            machine.AcceptCoin(2);
            machine.AcceptCoin(4);

            var result = machine.SellProduct(productName);

            result.Status.ShouldBe(eSellProductStatus.Success);
            result.Change.Sum(coin => coin.Key * coin.Value).ShouldBe(2);
        }

        [TestMethod]
        public void When_User_Inserts_Less_Than_Enough_Coins_And_Purchases_A_Product_Insufficient_Funds_Message_Is_Returned()
        {
            var productName = "Test";
            var testProduct = new Product() { Name = productName, Price = 5, Quantity = 1 };
            var machine = new VendingMachineService(null, null);
            machine.AddProduct(testProduct);

            machine.AcceptCoin(1);
            var result = machine.SellProduct(productName);

            result.Status.ShouldBe(eSellProductStatus.InsufficientFunds);
            result.Change.Any().ShouldBeFalse();
        }
        [TestMethod]
        public void When_User_Inserts_Enough_Coins_And_Purchases_A_Product_But_There_Are_No_More_Items_Sold_Out_Message_Is_Shown()
        {
            var productName = "Test";
            var testProduct = new Product() { Name = productName, Price = 1, Quantity = 0 };
            var machine = new VendingMachineService(null, null);
            machine.AddProduct(testProduct);

            machine.AcceptCoin(1);
            var result = machine.SellProduct(productName);

            result.Status.ShouldBe(eSellProductStatus.SoldOut);
            result.Change.Any().ShouldBeFalse();
        }
        [TestMethod]
        public void When_User_Inserts_More_Than_Enough_Coins_And_Purchases_A_Product_But_There_Are_Not_Enough_Coins_For_Change_An_Out_Of_Change_Message_Is_Shown()
        {
            var productName = "Test";
            var testProduct = new Product() { Name = productName, Price = 1, Quantity = 1 };
            var machine = new VendingMachineService(null, null);
            machine.AddProduct(testProduct);

            machine.AcceptCoin(5);
            var result = machine.SellProduct(productName);

            result.Status.ShouldBe(eSellProductStatus.OutOfChange);
            result.Change.Any().ShouldBeFalse();
        }
        [TestMethod]
        public void When_machine_state_is_json_saved_it_is_restored_correctly()
        {
            var machine = new VendingMachineService(new VendingMachineJsonLoader(), new VendingMachineJsonSaver());
            machine.AddCoin(1);
            machine.AddProduct(new Product() { Name = "Test", Price = 5, Quantity = 1 });
            machine.AcceptCoin(5);

            machine.SaveMachine().Wait();

            var loadedMachine = new VendingMachineService(new VendingMachineJsonLoader(), new VendingMachineJsonSaver());
            loadedMachine.LoadMachine().Wait();

            machine.CurrentInsertedAmount.ShouldBe(5);
            machine.Products.First().Name.ShouldBe("Test");
            machine.CoinsAvailable.First().Key.ShouldBe(1);
        }
    }
}
