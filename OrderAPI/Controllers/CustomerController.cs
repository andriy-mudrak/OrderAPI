using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int itemId = 0)
        {
            return Ok(await _customerService.Get(itemId));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerDTO value)
        {
            return Ok(await _customerService.Create(value));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] IEnumerable<CustomerDTO> value)
        {
            return Ok(await _customerService.Update(value));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] CustomerDTO value)
        {
            return Ok(await _customerService.Detele(value));
        }
    }
}
