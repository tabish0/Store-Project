﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; } = true;
    }
}
