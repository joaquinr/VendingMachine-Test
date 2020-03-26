using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services;

namespace VendingMachine.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendingMachineController : ControllerBase
    {
        private readonly ILogger<VendingMachineController> _logger;
        private readonly IVendingMachineService vendingMachine;

        public VendingMachineController(ILogger<VendingMachineController> logger, IVendingMachineService vendingMachine)
        {
            _logger = logger;
            this.vendingMachine = vendingMachine;
        }
        /// <summary>
        /// Gets the current client side machine state for its use in a UI
        /// </summary>
        [HttpGet("GetMachineState")]
        public async Task<ActionResult<VendingMachineData>> GetMachineState()
        {
            await this.vendingMachine.LoadMachine();
            var vendingMachineData = new VendingMachineData()
            {
                CurrentInsertedAmount = this.vendingMachine.CurrentInsertedAmount,
                AvailableProducts = this.vendingMachine.Products.Where(product => product.Quantity > 0).ToList(),
            };

            return Ok(vendingMachineData);
        }

        private List<Product> ProjectProductData()
        {
            return this.vendingMachine.Products;
        }

        /// <summary>
        /// Adds a coin to the user's input
        /// </summary>
        /// <param name="denomination"></param>
        [HttpPost("InsertCoin")]
        public async Task<IActionResult> AcceptCoin(double denomination)
        {
            await this.vendingMachine.LoadMachine();
            this.vendingMachine.AcceptCoin(denomination);
            await this.vendingMachine.SaveMachine();

            return Ok();
        }
        /// <summary>
        /// Returns coins inserted in the machine by the user
        /// </summary>
        /// <returns></returns>
        [HttpPost("ReturnCoins")]
        public async Task<ActionResult<Dictionary<double, int>>> ReturnCoins()
        {
            await this.vendingMachine.LoadMachine();
            var returnedCoins = this.vendingMachine.ReturnCoins();
            await this.vendingMachine.SaveMachine();

            return this.Ok(returnedCoins);
        }
        /// <summary>
        /// Sells a product to the customer.
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Status code for the operation and any change</returns>
        [HttpPost("SellProduct")]
        public async Task<ActionResult<SellProductResult>> SellProduct(string productName)
        {
            await this.vendingMachine.LoadMachine();
            var sellResult = this.vendingMachine.SellProduct(productName);
            await this.vendingMachine.SaveMachine();

            return Ok(sellResult);
        }


    }
}
