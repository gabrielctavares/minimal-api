using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Interfaces;

public interface IAdministradorService 
{        
    Administrador? Login(LoginDTO login);        
}
