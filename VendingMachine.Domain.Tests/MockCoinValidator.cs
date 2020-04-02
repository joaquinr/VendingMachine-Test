using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Domain.Services;

namespace VendingMachine.Domain.Tests
{
    public class MockCoinValidator : ICoinValidator
    {
        public bool IsValidCoin(double denomination)
        {
            return true;
        }
    }
}
