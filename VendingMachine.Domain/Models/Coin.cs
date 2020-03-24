using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Domain.Models
{
    public class Coin
    {
        public double Denomination { get; set; }
        public override string ToString()
        {
            return $"{this.Denomination} €";
        }
    }
}
