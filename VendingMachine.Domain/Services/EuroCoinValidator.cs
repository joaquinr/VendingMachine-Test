using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VendingMachine.Domain.Services
{
    public class EuroCoinValidator : ICoinValidator
    {
        public bool IsValidCoin(double denomination)
        {
            var acceptableCoins = new List<double>()
            {
                0.01,
                0.02,
                0.05,
                0.1,
                0.2,
                0.5,
                1,
                2
            };
            return acceptableCoins.Any(coin => Math.Round(coin, 2) == Math.Round(denomination, 2));
        }
    }
}
