using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Fake.Models.Shipment;
using BLL.Fake.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _itemService.GetAll());
        }
    }
}
