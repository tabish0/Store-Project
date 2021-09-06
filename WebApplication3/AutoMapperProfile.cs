using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Dto.CategoryDto;
using WebApplication3.Dto.ItemDto;
using WebApplication3.Models;

namespace WebApplication3
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Item, GetItem>();
            CreateMap<AddItem, Item>();
            CreateMap<Category, GetCategory>();
            CreateMap<AddCategory, Category>();
        }
    }
}
