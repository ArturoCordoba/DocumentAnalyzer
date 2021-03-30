using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

using DataHandlerSQL.Factory;
using AuthAPI.Services.SignupService;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SignupController : Controller
    {
        private readonly SignupService signupService;
        public SignupController(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.signupService = new SignupService(unitOfWorkFactory);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(SignupDto userData)
        {
            try
            {
                // Used the SigupService to determine if the request is valid
                SignupResult result = signupService.Signup(userData);

                switch (result)
                {
                    case SignupResult.EmailRegistered:
                        return Conflict("EMAILS_EXISTS");

                    case SignupResult.MissingInfo:
                        return BadRequest("MISSING_INFO");

                    case SignupResult.Success:
                        userData.password = null; 
                        return Created("user", userData);

                    default:
                        return BadRequest();
                }
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
