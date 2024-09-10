using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Infraestrutura.Db;

public class DbContexto : DbContext
{
    public DbSet<Administrador> Administradores { get; set; } = default!;
    public DbSet<Veiculo> Veiculos { get; set; } = default!;

    public DbContexto(DbContextOptions<DbContexto> options) : base(options)
    {
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