using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services
{
    public class VendingMachineJsonLoader : IVendingMachineLoader
    {
        public async Task LoadVendingMachine(VendingMachineState machine)
        {
            var json = await this.ReadTextAsync("machineState.json");
            var storedMachine = JsonConvert.DeserializeObject<VendingMachineState>(json);
            machine.CoinStorage = storedMachine.CoinStorage;
            machine.ProductStorage = storedMachine.ProductStorage;
            machine.UserCoinsAccepted = storedMachine.UserCoinsAccepted;
        }

        private async Task<string> ReadTextAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();

                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                return sb.ToString();
            }
        }
    }
}
