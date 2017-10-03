using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = "JwtBearer")]
        public IEnumerable<string> Get()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpPost]
        public IActionResult GenerateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var claims = new[]
                {
                          new Claim(JwtRegisteredClaimNames.Sub, "Test"),
                          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("workingwithdata2017"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken("workingwithdata2017",
                  "workingwithdata2017",
                  claims,
                  expires: DateTime.Now.AddMinutes(30),
                  signingCredentials: creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });

            }

            return BadRequest("Could not create token");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
