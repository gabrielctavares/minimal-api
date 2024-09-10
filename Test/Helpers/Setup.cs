using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Infraestrutura.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using MinimalApi.Dominio.Helpers;
using MinimalApi.Dominio.Entidades;
using Microsoft.Extensions.Configuration;

namespace Test.Helpers;

public class Setup
{
    public const string PORT = "5001";
    public static TestContext TestContext = default!;
    public static WebApplicationFactory<Program> Factory = default!;
    public static HttpClient Client = default!;
    public static void ClassInit(TestContext testContext)
    {
        TestContext = testContext;
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DbContexto>));

                    if (descriptor != null)
                    {
                        Console.WriteLine("Removendo o DbContexto");
                        services.Remove(descriptor);
                    }
                    
                    services.AddDbContext<DbContexto>(options =>
                    {
                        options.UseInMemoryDatabase("EstacionamentoTestMemory");
                    });
                });
            });

        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DbContexto>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        context.Database.EnsureCreated();  // Garante que o banco de dados em memória seja criado
        
        Client = Factory.CreateClient();
        var adm = new Administrador() {
            Nome = "Administrador",
            Email = "adm@teste.com",
            Senha = "123456",
            Perfil = "Adm"
        };

        var key = configuration.GetSection("Jwt:Key").Value ?? throw new Exception("Chave de segurança não configurada");

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MinimalApi.Dominio.Helpers.Helpers.GerarTokenJWT(adm, key));
    }

    public static void ClassCleanup()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}