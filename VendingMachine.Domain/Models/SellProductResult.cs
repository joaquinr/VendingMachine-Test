using System.Collections.Generic;

namespace VendingMachine.Domain.Models
{
    /// <summary>
    /// DTO to return sell product operation's status code and change in a single object
    /// </summary>
    public class SellProductResult
    {
        public Dictionary<double, int> Change { get; set; }
        public eSellProductStatus Status { get; set; }

        public SellProductResult()
        {
            this.Change = new Dictionary<double, int>();
        }
    }
}
