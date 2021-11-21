using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;
using DAL.Models;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.JWT
{
    public sealed class JwtGenerator : IJwtGenerator
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public JwtGenerator(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Name, user.UserName),
                new (JwtRegisteredClaimNames.Email, user.Email),
            };

            var roles = await _userManager.GetRolesAsync(user);
            var claimRoles = roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
            claims.AddRange(claimRoles);

            var jwtAppSettingOptions = _configuration.GetSection("JwtIssuerOptions");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAppSettingOptions["JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireTime = DateTime.Now.AddMinutes(Convert.ToDouble(jwtAppSettingOptions["JwtExpireMinutes"]));

            var token = new JwtSecurityToken(
                jwtAppSettingOptions["JwtIssuer"],
                jwtAppSettingOptions["JwtAudience"],
                claims,
                expires: expireTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
