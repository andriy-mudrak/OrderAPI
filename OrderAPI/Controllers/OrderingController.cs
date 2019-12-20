using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderingController : ControllerBase
    {
        private readonly IOrderingService _orderingService;

        public OrderingController(IOrderingService orderingService)
        {
            _orderingService = orderingService;
        }

        // POST: api/Ordering
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderingDTO model)
        {
            return Ok(await _orderingService.Create(model));
        }
    }
}
