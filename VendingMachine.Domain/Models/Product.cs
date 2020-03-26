using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Domain.Models
{
    /// <summary>
    /// Models a product inserted on the machine
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Product name, used as ID for the sell operation
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Product unit price
        /// </summary>
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
