using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Service
{
    public interface IGenericService<T1, T2>
    {
        public Task<ServiceResponse<List<T1>>> GetAll();
        public Task<ServiceResponse<T1>> GetById(int Id);
        public Task<ServiceResponse<List<T1>>> Insert(T2 obj);
        public Task<ServiceResponse<List<T1>>> Delete(int Id);

    }
}
