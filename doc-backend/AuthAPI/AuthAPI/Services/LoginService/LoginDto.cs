﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthAPI.Services.LoginService
{
    public class LoginDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
