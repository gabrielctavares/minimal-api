namespace MinimalApi.Dominio.ModelViews;

public struct Home
{
    public string Mensagem {get => "Bem-vindo ao Minimal API de Veiculos!";}    
    public readonly string Doc { get => "/swagger"; }
}
