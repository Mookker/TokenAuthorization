using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TokenAuthorization.Helpers
{
    public static class JwtGenerator
    {
        /// <summary>
        /// Generates short-lived token for protected controller
        /// </summary>
        /// <returns></returns>
        public static string GenerateProtectedToken(string key, string iss)
        {
            var secBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(secBytes);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var nbf = DateTime.UtcNow;
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, Constants.ProtectedUserRole));
            
            JwtSecurityToken token = new JwtSecurityToken(
                iss,
                "",
                claims,
                nbf,
                nbf.AddSeconds(300),
                signingCredentials
            );

            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(token);
            return tokenString;
        }
    }
}