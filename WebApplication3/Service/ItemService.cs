using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Data;
using WebApplication3.Dto.ItemDto;
using WebApplication3.Models;

namespace WebApplication3.Service
{
    public class ItemService : IGenericService<GetItem, AddItem>
    {
        private readonly StoreContext context;
        private readonly IMapper mapper;

        public ItemService(StoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public Task<ServiceResponse<List<GetItem>>> Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<GetItem>>> GetAll()
        {
            ServiceResponse<List<GetItem>> response = new ServiceResponse<List<GetItem>>();
            response.Data = await context.Items.Select(i => mapper.Map<GetItem>(i)).ToListAsync();
            return response;
        }

        public Task<ServiceResponse<GetItem>> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<GetItem>>> Insert(AddItem obj)
        {
            ServiceResponse<List<GetItem>> response = new ServiceResponse<List<GetItem>>();
            context.Items.Add(mapper.Map<Item>(obj));
            await context.SaveChangesAsync();
            response.Data = await context.Items.Select(i => mapper.Map<GetItem>(i)).ToListAsync();
            return response;
        }
    }
}
