using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Servicos;

public class VeiculoService : IVeiculoService
{
    private readonly DbContexto _dbContexto;
    public VeiculoService(DbContexto dbContexto)
    {
        _dbContexto = dbContexto;
    }
    public Veiculo Atualizar(Veiculo veiculo)
    {
        _dbContexto.Veiculos.Update(veiculo);
        _dbContexto.SaveChanges();
        return veiculo;
    }

    public void Excluir(Veiculo veiculo)
    {
        _dbContexto.Veiculos.Remove(veiculo);
        _dbContexto.SaveChanges();
        throw new NotImplementedException();
    }

    public Veiculo Inserir(Veiculo veiculo)
    {
        _dbContexto.Veiculos.Add(veiculo);
        _dbContexto.SaveChanges();
        return veiculo;
    }

    public Veiculo? PorId(int id)
    {
        return _dbContexto.Veiculos.Find(id);
    }

    public List<Veiculo> Todos(int pagina = 1, string? nome = null, string? marca = null)
    {
        var query = _dbContexto.Veiculos.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
            query = query.Where(x => x.Nome.ToLower().Contains(nome));
        
        if (!string.IsNullOrEmpty(marca))
            query = query.Where(x => x.Marca.ToLower().Contains(marca));

        int quantidadePorPagina = 10;
        query = query.Skip((pagina - 1) * quantidadePorPagina).Take(quantidadePorPagina);
        return query.ToList();
    }
}