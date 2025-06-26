using ArlequimPetShop.SharedKernel;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace ArlequimPetShop.Tests.SharedKernels;

/// <summary>
/// Testes unitários para a classe BaseHttpClient, focando no envio de requisições HTTP.
/// </summary>
[TestFixture]
public class BaseHttpClientTests
{
    /// <summary>
    /// Verifica se o token Bearer é corretamente incluído no header da requisição HTTP.
    /// </summary>
    [Test]
    public async Task Send_ShouldIncludeBearerToken_WhenProvided()
    {
        // Arrange
        var expectedContent = new { ok = true };
        var json = JsonConvert.SerializeObject(expectedContent);

        var httpMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Headers.Authorization != null &&
                    req.Headers.Authorization.Scheme == "Bearer" &&
                    req.Headers.Authorization.Parameter == "test-token"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpMessage)
            .Verifiable();

        var loggerMock = new Mock<ILogger<BaseHttpClient>>();
        var httpClient = new HttpClient(handlerMock.Object);

        var client = new BaseHttpClient(loggerMock.Object);
        typeof(BaseHttpClient)
            .GetField("_client", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(client, httpClient);

        // Act
        var response = await client.Send<dynamic>(
            HttpMethod.Get,
            "https://www.google.com.br/",
            saveLog: false,
            bearer: "test-token"
        );

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsTrue((bool)response.Data.ok);

        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(r =>
                r.Headers.Authorization != null &&
                r.Headers.Authorization.Scheme == "Bearer" &&
                r.Headers.Authorization.Parameter == "test-token"),
            ItExpr.IsAny<CancellationToken>());
    }
}