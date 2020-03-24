using System.Collections.Generic;

namespace VendingMachine.Domain
{
    public class SellProductResult
    {
        public List<Coin> Change { get; set; }
        public eSellProductStatus Status { get; set; }

        public SellProductResult()
        {
            this.Change = new List<Coin>();
        }
    }
}
