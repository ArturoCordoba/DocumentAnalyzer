using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLibrary
{
    public interface IAuthenticationService
    {
        public string Authenticate(string email, string password);

    }
}
