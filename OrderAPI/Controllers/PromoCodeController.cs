using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoCodeController : Controller
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodeController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int itemId = 0)
        {
            return Ok(await _promoCodeService.Get(itemId)); 
        }
 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PromoCodeDTO value)
        {
            return Ok(await _promoCodeService.Create(value));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] IEnumerable<PromoCodeDTO> value)
        {
            return Ok(await _promoCodeService.Update(value));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] PromoCodeDTO value)
        {
            return Ok(await _promoCodeService.Detele(value));
        }
    }
}
