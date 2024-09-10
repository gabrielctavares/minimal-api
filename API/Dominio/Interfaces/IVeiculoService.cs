using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Interfaces;

public interface IVeiculoService 
{        
    List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null);        
    Veiculo? PorId(int id);
    Veiculo Inserir(Veiculo veiculo);
    Veiculo Atualizar(Veiculo veiculo);
    void Excluir(Veiculo veiculo);
}
