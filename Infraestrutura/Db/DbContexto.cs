using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Infraestrutura.Db;

public class DbContexto : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Administrador> Administradores { get; set; } = default!;

    public DbContexto(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;


        var stringConexao = _configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(stringConexao))
        {
            optionsBuilder.UseSqlServer(stringConexao);                    
        }
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>().HasData(
            new Administrador { 
                Id = 1, 
                Nome = "Administrador", 
                Email = "administrador@teste.com", 
                Senha = "123456", 
                Perfil = "Adm" 
            }
        );        
    }

}