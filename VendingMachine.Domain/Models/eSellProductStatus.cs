namespace VendingMachine.Domain.Models
{
    public enum eSellProductStatus
    {
        Success = 0,
        InsufficientFunds = 1,
        SoldOut = 2,
        OutOfChange = 3,
    }
}
