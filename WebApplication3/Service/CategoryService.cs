using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Data;
using WebApplication3.Dto.CategoryDto;
using WebApplication3.Models;

namespace WebApplication3.Service
{
    public class CategoryService : IGenericService<GetCategory, AddCategory>
    {
        StoreContext context;
        IMapper mapper;
        public CategoryService(StoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public Task<ServiceResponse<List<GetCategory>>> Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<GetCategory>>> GetAll()
        {
            ServiceResponse<List<GetCategory>> response = new ServiceResponse<List<GetCategory>>();
            response.Data = await context.Categories.Select(c => mapper.Map<GetCategory>(c)).ToListAsync();
            return response;
        }

        public Task<ServiceResponse<GetCategory>> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<GetCategory>>> Insert(AddCategory obj)
        {
            ServiceResponse<List<GetCategory>> response = new ServiceResponse<List<GetCategory>>();
            Category c = mapper.Map<Category>(obj);
            context.Add(c);
            await context.SaveChangesAsync();
            response.Data = await context.Categories.Select(c => mapper.Map<GetCategory>(c)).ToListAsync();
            return response;
        }
    }
}
