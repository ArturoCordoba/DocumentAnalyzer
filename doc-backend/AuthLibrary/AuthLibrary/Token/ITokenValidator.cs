﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLibrary.Token
{
    public interface ITokenValidator
    {
        public bool VerifyToken(string token);
    }
}
