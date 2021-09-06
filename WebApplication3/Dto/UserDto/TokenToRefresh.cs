using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Dto.UserDto
{
    public class TokenToRefresh
    {
        public string JwtToken { get; set; }
        public string refreshToken { get; set; }
    }
}
