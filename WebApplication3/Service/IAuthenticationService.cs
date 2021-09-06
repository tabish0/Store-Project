using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Dto.UserDto;
using WebApplication3.Models;

namespace WebApplication3.Service
{
    public interface IAuthenticationService
    {
        public Task<ServiceResponse<int>> Register(RegisterUser obj);
        public Task<ServiceResponse<string>> Login(LoginUser obj);
    }
}
