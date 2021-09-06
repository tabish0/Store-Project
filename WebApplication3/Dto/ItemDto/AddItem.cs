using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Dto.ItemDto
{
    public class AddItem
    {
        public string ItemName { get; set; }
        public int CategoryId { get; set; }
    }
}
