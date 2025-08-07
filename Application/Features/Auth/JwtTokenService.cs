using Application.Features.Auth.Common;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth;


public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string userId, string userName, string role, string fullName, string phoneNumber)
    {
        //    Issuer = _configuration["Jwt:Issuer"],
        //    Audience = _configuration["Jwt:Audience"]
        //};
        //var token = tokenHandler.CreateToken(tokenDescriptor);

        var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role),
                new Claim("FullName", fullName),
                new Claim("PhoneNumber", phoneNumber),
            };
        var ket = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(2),
                    signingCredentials: creds,
                    issuer: _configuration["Jwt:Issuer"],
                    audience : _configuration["Jwt:Audience"]
                );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

