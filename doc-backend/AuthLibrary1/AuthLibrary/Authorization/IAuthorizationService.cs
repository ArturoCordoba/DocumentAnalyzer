using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLibrary.Authorization
{
    interface IAuthorizationService
    {
        public string Authorize(string token);

    }
}
