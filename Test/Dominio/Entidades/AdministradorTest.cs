using MinimalApi.Dominio.Entidades;

namespace Test.Dominio.Entidades;

[TestClass]
public class AdministradorTest 
{
    // // deve criar administrador
    [TestMethod]
    public void DeveCriarAdministrador()
    {
        //arrange
        var administrador = new Administrador();

        //act
        administrador.Nome = "Administrador";
        administrador.Email = "admin@gmail.com";
        administrador.Senha = "123456";
        
        //assert
        Assert.AreEqual("Administrador", administrador.Nome);
        Assert.AreEqual("admin@gmail.com", administrador.Email);
        Assert.AreEqual("123456", administrador.Senha);
    }
    
}