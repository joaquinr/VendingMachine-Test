using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services
{
    public interface IVendingMachineLoader
    {
        Task LoadVendingMachine(VendingMachineState machine);
    }
}