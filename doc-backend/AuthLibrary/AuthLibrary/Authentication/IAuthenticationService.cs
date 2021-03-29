using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLibrary.Authentication
{
    public interface IAuthenticationService
    {
        public string Authenticate(string email, string password, string correctEmail, string correctPassword);

    }
}
