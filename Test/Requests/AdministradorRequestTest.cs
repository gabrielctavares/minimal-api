using Microsoft.AspNetCore.Mvc.Testing;

[TestClass]
public class AdministradorRequestTest
{
    private static WebApplicationFactory<Program> _factory = default!;
    private static HttpClient _client = default!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestMethod]
    public async Task GetAdministrador_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/administrador");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(responseString.Contains("expected content"));
    }

    [TestMethod]
    public async Task PostAdministrador_ReturnsCreatedStatusCode()
    {
        // Arrange
        var content = new StringContent("{\"name\":\"Test Admin\"}", System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/administrador", content);

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [TestMethod]
    public async Task PutAdministrador_ReturnsNoContentStatusCode()
    {
        // Arrange
        var content = new StringContent("{\"name\":\"Updated Admin\"}", System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/administrador/1", content);

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [TestMethod]
    public async Task DeleteAdministrador_ReturnsNoContentStatusCode()
    {
        // Act
        var response = await _client.DeleteAsync("/administrador/1");

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [TestMethod]
    public async Task GetAdministrador_ReturnsNotFoundStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/administrador/999");

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task PostAdministrador_ReturnsBadRequestStatusCode()
    {
        // Arrange
        var content = new StringContent("{\"invalid\":\"data\"}", System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/administrador", content);

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task PutAdministrador_ReturnsBadRequestStatusCode()
    {
        // Arrange
        var content = new StringContent("{\"invalid\":\"data\"}", System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/administrador/1", content);

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task DeleteAdministrador_ReturnsNotFoundStatusCode()
    {
        // Act
        var response = await _client.DeleteAsync("/administrador/999");

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}