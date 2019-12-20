using System.Threading.Tasks;
using BLL.Fake.Models.Shipment;
using BLL.Fake.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentMethodService _shipmentMethodService;

        public ShipmentController(IShipmentMethodService shipmentMethodService)
        {
            _shipmentMethodService = shipmentMethodService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody]ShipmentModel shipment)
        {
            return Ok(await _shipmentMethodService.GetMethods(shipment));
        }

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody]ShipmentRequest shipment)
        //{
        //    return Ok(await _shipmentMethodService.CreateShipment(shipment));
        //}
    }
}