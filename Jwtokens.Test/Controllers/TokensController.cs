using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jwtokens.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private JwtService Service { get; }

        public TokensController(JwtService service)
        {
            Service = service;
        }

        [HttpGet("Auth")]
        [Authorize]
        public IActionResult Auth()
        {
            return Ok("You're authenticated!");
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return Service.CreateToken(new Claim("Testing", "Test"));
        }

        [HttpGet("{token}")]
        public IEnumerable<Claim> Validate([FromRoute] string token)
        {
            return Service.DeserializeToken(token).ClaimsPrincipal.Claims;
        }
    }
}