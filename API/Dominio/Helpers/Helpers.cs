using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Helpers;

public static class Helpers
{
    public static string GerarTokenJWT(Administrador administrador, string key){
    var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        expires: DateTime.Now.AddDays(1),
        issuer: "minimalapi",
        audience: "minimalapi",
        claims: new[] {
            new Claim(ClaimTypes.Email, administrador.Email),
            new Claim(ClaimTypes.Role, administrador.Perfil),
            new Claim("Perfil", administrador.Perfil),
        },
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}
}