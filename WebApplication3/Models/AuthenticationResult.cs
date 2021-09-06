using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class AuthenticationResult
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
