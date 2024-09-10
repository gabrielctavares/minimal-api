using MinimalApi.Dominio.Entidades;

namespace Test.Dominio.Entidades;

[TestClass]
public class VeiculoTest
{

    [TestMethod]
    public void DeveCriarVeiculo()
    {
        //arrange
        var veiculo = new Veiculo();

        //act
        veiculo.Id = 1;
        veiculo.Nome = "Uno";
        veiculo.Marca = "Fiat";
        veiculo.Ano = 2020;

        //assert
        Assert.AreEqual(1, veiculo.Id);
        Assert.AreEqual("Fiat", veiculo.Marca);
        Assert.AreEqual("Uno", veiculo.Nome);
        Assert.AreEqual(2020, veiculo.Ano);
    }
}