using MinimalApi.Dominio.Enuns;

namespace MinimalApi.Dominio.ModelViews;

public record AdministradorLogado(string Email, string Perfil, string Token);