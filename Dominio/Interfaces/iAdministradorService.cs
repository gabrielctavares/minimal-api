using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Interfaces;

public interface IAdministradorService 
{        
    Administrador? Login(LoginDTO login);    
    Administrador Incluir(Administrador administrador);
    Administrador? BuscaPorId(int id);
    List<Administrador> Todos(int? pagina);    
}
