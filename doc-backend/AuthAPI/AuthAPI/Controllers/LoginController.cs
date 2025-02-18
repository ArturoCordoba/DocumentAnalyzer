﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

using DataHandlerSQL.Factory;
using AuthAPI.Services.LoginService;
using AuthLibrary.Factory;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IAuthServiceFactory authServiceFactory;

        public LoginController(IUnitOfWorkFactory unitOfWorkFactory, IAuthServiceFactory authServiceFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.authServiceFactory = authServiceFactory;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginDto requestData)
        {
            try
            {
                LoginService loginService = new LoginService(unitOfWorkFactory, authServiceFactory);

                // Uses the LoginService to determine if the request is valid
                string token = loginService.Login(requestData);

                if(token == null) // Invalid information
                {
                    return Unauthorized();
                }

                return Ok(token); // Valid info
            } catch
            {
                return BadRequest();
            }
        }
    }
}
