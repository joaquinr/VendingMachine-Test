using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Domain.Models
{
    public class VendingMachineState
    {
        public List<Coin> CoinStorage { get; set; }
        public List<Product> ProductStorage { get; set; }
        public List<Coin> UserCoinsAccepted { get; set; }

        public VendingMachineState()
        {
            this.CoinStorage = new List<Coin>();
            this.ProductStorage = new List<Product>();
            this.UserCoinsAccepted = new List<Coin>();
        }
    }
}
