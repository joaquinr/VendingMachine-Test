using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Domain.Models
{
    /// <summary>
    /// Current vending machine state, including coins inserted by the user, coins in the machine and products stored
    /// </summary>
    public class VendingMachineState
    {
        public Dictionary<double, int> CoinStorage { get; set; }
        public List<Product> ProductStorage { get; set; }
        public Dictionary<double, int> UserCoinsAccepted { get; set; }

        public VendingMachineState()
        {
            this.CoinStorage = new Dictionary<double, int>();
            this.ProductStorage = new List<Product>();
            this.UserCoinsAccepted = new Dictionary<double, int>();
        }
    }
}
