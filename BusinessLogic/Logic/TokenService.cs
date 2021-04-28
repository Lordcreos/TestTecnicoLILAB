﻿using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        private readonly IConfiguration _config;

        public TokenService(IConfiguration config) {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }

        public string CreateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Name, usuario.Nombre),
                new Claim(JwtRegisteredClaimNames.FamilyName, usuario.Apellido),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("username", usuario.UserName),
            };

            var credencials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenConfiguration = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(60),
                SigningCredentials = credencials,
                Issuer = _config["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfiguration);

            return tokenHandler.WriteToken(token);
        }
    }
}
