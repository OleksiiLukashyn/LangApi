using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LangApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LangApi.Controllers
{
    public class TokenController : ControllerBase
    {
        [HttpPost("api/token")]
        public IActionResult CreateToken(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                // заглушка для проверки идентичности
                if (userName != "" && password != "")
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(LangTokenOptions.Key));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, userName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, userName)
                    };

                    var token = new JwtSecurityToken(
                            LangTokenOptions.Issuer,
                            LangTokenOptions.Audience,
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(60),
                            signingCredentials: creds
                        );
                    var result = new
                    {
                        access_token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };
                    return Created("api/token", result);
                }
                else
                {
                    return Forbid();
                }
            }
            return BadRequest(ModelState);
        }
    }
}