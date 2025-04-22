using Microsoft.IdentityModel.Tokens;
using OnlineBookExchange.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace OnlineBookExchange.Services
{
    public class Authentication
    {
        public static string GenerateJWTAuthetication(int userId, string userName, string role)
        {
            var claims = new List<Claim>
        {
             new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
             new Claim(ClaimTypes.Name, userName),
             new Claim(ClaimTypes.Role, role),
             // Additional standard claims if needed
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["config:JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(
                Convert.ToDouble(ConfigurationManager.AppSettings["config:JwtExpireDays"]));

            var token = new JwtSecurityToken(
                issuer: ConfigurationManager.AppSettings["config:JwtIssuer"],
                audience: ConfigurationManager.AppSettings["config:JwtAudience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static ClaimsPrincipal ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["config:JwtKey"]);
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = ConfigurationManager.AppSettings["config:JwtIssuer"],
                    ValidateAudience = true,
                    ValidAudience = ConfigurationManager.AppSettings["config:JwtAudience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Validate and get principal
                //var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                //// Retrieve username from the correct claim
                //var userName = principal.FindFirst(ClaimTypes.Name)?.Value;
                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }
    }
}