using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Dto.CategoryDto;
using WebApplication3.Service;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericService<GetCategory, AddCategory> genericService;

        public CategoryController(IGenericService<GetCategory, AddCategory> genericService)
        {
            this.genericService = genericService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await genericService.GetAll());
        }
        [HttpPost]
        public async Task<IActionResult> Insert(AddCategory obj)
        {
            return Ok(await genericService.Insert(obj));
        }
    }
}
