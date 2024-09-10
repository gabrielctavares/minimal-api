using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Enuns;
using MinimalApi.Dominio.ModelViews;
using Test.Helpers;

[TestClass]
public class AdministradorRequestTest
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        Setup.ClassInit(context);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }


//app.MapGet("/administradores",
//app.MapGet("/Administradores/{id}", 
    [TestMethod]
    public async Task PostLogin_ReturnsSuccessStatusCode()
    {
        // Arrange
        var loginDTO = new LoginDTO(){
            Email = "administrador@teste.com",
            Senha = "123456"
        };
        var content = new StringContent(JsonSerializer.Serialize(loginDTO), System.Text.Encoding.UTF8, "application/json");
        // Act        
        var response = await Setup.Client.PostAsync("/administradores/login", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var administradorLogado = JsonSerializer.Deserialize<AdministradorLogado>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.IsTrue(administradorLogado?.Token?.Length > 0);
    }

    [TestMethod]
    public async Task PostAdministrador_ReturnsCreatedStatusCode()
    {
        // Arrange
        var administrador = new AdministradorDTO(){ 
            Nome = "Teste 02",           
            Email = "teste02@adm.com",   
            Senha = "teste",
            Perfil = Perfil.Adm
        };
        var content = new StringContent(JsonSerializer.Serialize(administrador), System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await Setup.Client.PostAsync("/administradores", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    // [TestMethod]
    // public async Task PutAdministrador_ReturnsNoContentStatusCode()
    // {
    //     // Arrange
    //     var content = new StringContent("{\"name\":\"Updated Admin\"}", System.Text.Encoding.UTF8, "application/json");

    //     // Act
    //     var response = await Setup.Client.PutAsync("/administrador/1", content);

    //     // Assert
    //     Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    // }

    // [TestMethod]
    // public async Task DeleteAdministrador_ReturnsNoContentStatusCode()
    // {
    //     // Act
    //     var response = await Setup.Client.DeleteAsync("/administrador/1");

    //     // Assert
    //     Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    // }

    [TestMethod]
    public async Task GetAdministrador_ReturnsNotFoundStatusCode()
    {
        // Act
        var response = await Setup.Client.GetAsync("/administradores/999");

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task GetAdministrador_ReturnsOk()
    {
        // Act
        var response = await Setup.Client.GetAsync("/administradores/1");

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task GetAdministradors_ReturnsOk()
    {
        // Act
        var response = await Setup.Client.GetAsync("/administradores");

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task PostAdministrador_ReturnsBadRequestStatusCode()
    {
        // Arrange
        var content = new StringContent("{\"invalid\":\"data\"}", System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await Setup.Client.PostAsync("/administradores", content);

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    // [TestMethod]
    // public async Task PutAdministrador_ReturnsBadRequestStatusCode()
    // {
    //     // Arrange
    //     var content = new StringContent("{\"invalid\":\"data\"}", System.Text.Encoding.UTF8, "application/json");

    //     // Act
    //     var response = await Setup.Client.PutAsync("/administrador/1", content);

    //     // Assert
    //     Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    // }

    // [TestMethod]
    // public async Task DeleteAdministrador_ReturnsNotFoundStatusCode()
    // {
    //     // Act
    //     var response = await Setup.Client.DeleteAsync("/administrador/999");

    //     // Assert
    //     Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    // }
}