using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class RefreshToken
    {
        public int UserID { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsBlackListed { get; set; } = true;
        public User User { get; set; }
    }
}
