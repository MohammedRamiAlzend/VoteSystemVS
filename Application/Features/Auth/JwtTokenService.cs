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

    public string GenerateToken(string userId, string role, string? fullName = null, string? phoneNumber=null, string? email = null)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, role),

        ];

        if (fullName != null)
        {
            claims.Add(new Claim(ClaimTypes.Name, fullName));
        }
        if (phoneNumber != null)
        {
            claims.Add(new Claim("PhoneNumber", phoneNumber));
        }
        if (email != null)
        {
            claims.Add(new Claim("Email", email));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
                    claims: claims,
                    expires: role=="Admin"? DateTime.UtcNow.AddDays(2) : DateTime.UtcNow.AddDays(1),
                    signingCredentials: creds,
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"]
                );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

