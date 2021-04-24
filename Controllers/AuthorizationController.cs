using Authorization.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Authorization.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IJWTAuthenticationManager jwtAuthenticationManager;

        public AuthorizationController(IJWTAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] PortfolioDetail customer)
        {

            var tokenString = jwtAuthenticationManager.Authenticate(customer.Email, customer.Password);

            if (tokenString == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(new { token = tokenString,email = customer.Email });
            }

        }
       
        [HttpGet("GetCustomerByEmail/{email}")]
        public ActionResult GetCustomerByEmail(string email)
        {
            var custObj = jwtAuthenticationManager.GetCustomer(email);
            if (custObj == null)
            {
                return BadRequest();
            }
            return Ok(custObj);
        }

    }
}
