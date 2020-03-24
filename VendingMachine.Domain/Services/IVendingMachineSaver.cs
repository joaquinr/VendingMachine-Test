using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services
{
    public interface IVendingMachineSaver
    {
        Task SaveMachine(VendingMachineState machine);
    }
}