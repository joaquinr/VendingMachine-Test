namespace VendingMachine.Domain.Services
{
    public interface ICoinValidator
    {
        bool IsValidCoin(double denomination);
    }
}