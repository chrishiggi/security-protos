using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JwtAuthApi.Models;
using JwtAuthApi.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;

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
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public IEnumerable<string> Get() 
        {
            return new string[] { "California", "Nevada" };
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult Authenticate([FromBody]UserCred userCred)
        {
            var token = _jwtAuthenticationManager.Authenticate(userCred.Username, userCred.Password);

            if (token.IsNullOrEmpty())
            {
                Unauthorized(new ErrorResponse 
                {
                    Message = "Unable to Authorize",
                    Code = StatusCodes.Status401Unauthorized.ToString(),
                });
            }
            return Ok(token);
        }
    }
}