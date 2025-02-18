﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AuthLibrary.Authorization;
using AuthLibrary.Token;

namespace AuthLibrary.Factory
{
    public interface IAuthServiceFactory
    {
        public IAuthorizationService Authorization { get; }

        public ITokenGenerator TokenGenerator { get; }
    }
}
