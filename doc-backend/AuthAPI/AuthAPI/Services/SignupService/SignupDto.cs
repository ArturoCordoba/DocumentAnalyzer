using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthAPI.Services.SignupService
{
    public class SignupDto
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
