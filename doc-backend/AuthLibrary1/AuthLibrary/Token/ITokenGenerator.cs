using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLibrary
{
    public interface ITokenGenerator
    {
        public string GenerateToken(string email);
    }
}
