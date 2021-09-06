using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication3.Dto.UserDto;
using WebApplication3.Models;
using WebApplication3.Service;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterUser obj)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            response = await authenticationService.Register(obj);
            if (response.Success == true)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginUser obj)
        {
            ServiceResponse<AuthenticationResult> response = new ServiceResponse<AuthenticationResult>();
            response = await authenticationService.Login(obj);
            if (response.Success == true)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh(TokenToRefresh obj)
        {
            ServiceResponse<AuthenticationResult> response = new ServiceResponse<AuthenticationResult>();
            response = await authenticationService.RefreshToken(obj);
            if (response.Success == true)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            response = await authenticationService.Logout();
            if (response.Success == true)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
