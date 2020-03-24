using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace VendingMachine.Domain.Tests
{
    [TestClass]
    public class VendingMachineTests
    {
        [TestMethod]
        public void When_User_Inserts_A_Coin_Vending_Machine_Stores_It()
        {
            var machine = new VendingMachine(null);

            machine.AcceptCoin(new Coin() { Denomination = 0.5 });

            machine.CurrentInsertedAmount.ShouldBe(0.5);
        }
        [TestMethod]
        public void When_User_Asks_To_Return_Coins_Coins_Are_Returned()
        {
        }
        [TestMethod]
        public void When_User_Inserts_Just_Enough_Coins_And_Purchases_A_Product_Success_Message_Is_Shown()
        {
        }
        [TestMethod]
        public void When_User_Inserts_More_Than_Enough_Coins_And_Purchases_A_Product_Success_Message_Is_Returned_And_Change_Is_Returned()
        {
        }
        [TestMethod]
        public void When_User_Inserts_Less_Than_Enough_Coins_And_Purchases_A_Product_Insufficient_Funds_Message_Is_Returned()
        {
        }
        [TestMethod]
        public void When_User_Inserts_Enough_Coins_And_Purchases_A_Product_But_There_Are_No_More_Items_Sold_Out_Message_Is_Shown()
        {
        }
        [TestMethod]
        public void When_User_Inserts_More_Than_Enough_Coins_And_Purchases_A_Product_But_There_Are_Not_Enough_Coins_For_Change_An_Out_Of_Change_Message_Is_Shown()
        {
        }

    }
}
