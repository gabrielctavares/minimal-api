using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Enuns;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;

#region Builder
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(key))
    throw new Exception("Chave de segurança não configurada");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =  JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option => 
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,  
        ValidateAudience = false,   
        ValidateIssuer = false,   
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key))
    };
});
builder.Services.AddScoped<IAdministradorService, AdministradorService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,        
        Description = "Insira o Token JWT aqui"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        } 
    });
});
builder.Services.AddAuthorization();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home()))
.AllowAnonymous()
.WithTags("Home");
#endregion

#region Administradores
string GerarTokenJWT(Administrador administrador){
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

app.MapPost("/administradores/login", ([FromBody] LoginDTO login, IAdministradorService service) =>
{
    var administrador = service.Login(login);
    if(administrador is null) 
        return Results.Unauthorized();
    
    var token = GerarTokenJWT(administrador);
    return Results.Ok(new AdministradorLogado(administrador.Email, administrador.Perfil, token));
})
.AllowAnonymous()
.WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorService service) => {
    var adms = new List<AdministradorModelView>();
    var administradores = service.Todos(pagina);
    foreach(var adm in administradores)
    {
        adms.Add(new AdministradorModelView{
            Id = adm.Id,
            Email = adm.Email,
            Perfil = adm.Perfil
        });
    }
    return Results.Ok(adms);
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm"})
.RequireAuthorization()
.WithTags("Administradores");

app.MapGet("/Administradores/{id}", ([FromRoute] int id, IAdministradorService service) => {
    var administrador = service.BuscaPorId(id);
    if(administrador == null) return Results.NotFound();
    return Results.Ok(new AdministradorModelView{
            Id = administrador.Id,
            Email = administrador.Email,
            Perfil = administrador.Perfil
    });
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm"})
.RequireAuthorization()
.WithTags("Administradores");

app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorService service) => {
    var validacao = new ErrosValidacao{
        Mensagens = new List<string>()
    };

    if(string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Mensagens.Add("Email não pode ser vazio");
    if(string.IsNullOrEmpty(administradorDTO.Senha))
        validacao.Mensagens.Add("Senha não pode ser vazia");
    if(administradorDTO.Perfil == null)
        validacao.Mensagens.Add("Perfil não pode ser vazio");

    if(validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);
    
    var administrador = new Administrador{
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha,
        Perfil = administradorDTO.Perfil.ToString() ?? Perfil.Editor.ToString()
    };

    service.Incluir(administrador);

    return Results.Created($"/administrador/{administrador.Id}", new AdministradorModelView{
        Id = administrador.Id,
        Email = administrador.Email,
        Perfil = administrador.Perfil
    });
    
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm"})
.RequireAuthorization()
.WithTags("Administradores");
#endregion  

#region Veiculos

ErrosValidacao validaDTO(VeiculoDTO veiculoDTO){
    var erros = new ErrosValidacao() {
        Mensagens = new List<string>()
    };

    if (string.IsNullOrEmpty(veiculoDTO.Nome))
        erros.Mensagens.Add("Nome é obrigatório");

    if (string.IsNullOrEmpty(veiculoDTO.Marca))
        erros.Mensagens.Add("Marca é obrigatória");

    if (veiculoDTO.Ano < 1900)
        erros.Mensagens.Add("Ano inválido");

    return erros;
}
app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, [FromServices] IVeiculoService service) => {
    var erros = validaDTO(veiculoDTO);
    
    if (erros.Mensagens.Count > 0)
        return Results.BadRequest(erros);

    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    }; 
    service.Inserir(veiculo);

    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);    
})
.RequireAuthorization()
.WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, [FromServices] IVeiculoService service) => {        
    return Results.Ok(service.Todos(pagina));
})
.RequireAuthorization()
.WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromServices] IVeiculoService service, [FromRoute] int id) => {
    var veiculo = service.PorId(id);

    if (veiculo is null)
        return Results.NotFound();

    return Results.Ok(veiculo);
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm,Editor"})
.RequireAuthorization()
.WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromBody] VeiculoDTO veiculoDTO, [FromServices] IVeiculoService service, [FromRoute] int id) => {
    var veiculo = service.PorId(id);

    if (veiculo is null)
        return Results.NotFound();

    var erros = validaDTO(veiculoDTO);
    if (erros.Mensagens.Count > 0)
        return Results.BadRequest(erros);

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;
    service.Atualizar(veiculo);

    return Results.Ok(veiculo);
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm"})
.RequireAuthorization()
.WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromServices] IVeiculoService service, int id) => {
    var veiculo = service.PorId(id);

    if (veiculo is null)
        return Results.NotFound();
    
    service.Excluir(veiculo);
    return Results.NoContent();
})
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm"})
.RequireAuthorization()
.WithTags("Veiculos");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion


public partial class Program { } // Torna a classe acessível
