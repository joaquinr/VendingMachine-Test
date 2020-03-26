namespace VendingMachine.Domain.Models
{
    /// <summary>
    /// Current result of the sell product operation
    /// </summary>
    public enum eSellProductStatus
    {
        Success = 0,
        InsufficientFunds = 1,
        SoldOut = 2,
        OutOfChange = 3,
    }
}
