using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JwtAuthApi.Models;
using JwtAuthApi.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuthApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NameController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public NameController(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet]
        public IEnumerable<string> Get() 
        {
            return new string[] { "California", "Nevada" };
        }

        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserCred userCred)
        {
            var token = _jwtAuthenticationManager.Authenticate(userCred.Username, userCred.Password);

            if (token.IsNullOrEmpty())
            {
                Unauthorized();
            }
            return Ok(token);
        }
    }
}