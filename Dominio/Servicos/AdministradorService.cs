using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Servicos
{
    public class AdministradorService : IAdministradorService
    {
        private readonly DbContexto _dbContexto;
        public AdministradorService(DbContexto dbContexto)
        {
            _dbContexto = dbContexto;
        }

        public Administrador? Login(LoginDTO login)
        {
            return _dbContexto.Administradores
                .FirstOrDefault(x => x.Email == login.Email && x.Senha == login.Senha);
        }
    }
}