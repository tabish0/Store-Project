using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Dto.ItemDto;
using WebApplication3.Service;

namespace WebApplication3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IGenericService<GetItem, AddItem> genericService;

        public ItemController(IGenericService<GetItem, AddItem> genericService)
        {
            this.genericService = genericService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await genericService.GetAll());
        }
        [HttpPost]
        public async Task<IActionResult> Insert(AddItem obj)
        {
            return Ok(await genericService.Insert(obj));
        }
    }
}
