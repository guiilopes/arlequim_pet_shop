using ArlequimPetShop.SharedKernel.Options;
using Microsoft.Extensions.Options;
using Moq;

namespace ArlequimPetShop.Tests.SharedKernels.Options
{
    /// <summary>
    /// Testes para a classe de configuração AppSettingsOptions.
    /// </summary>
    [TestFixture]
    public class AppSettingsOptionsTests
    {
        [Test]
        public void OptionsMonitor_ShouldReturnConfiguredApiUrl()
        {
            // Arrange
            var expectedUrl = "https://api.arlequim.com.br";
            var mockOptions = new Mock<IOptionsMonitor<AppSettingsOptions>>();
            mockOptions.Setup(o => o.CurrentValue).Returns(new AppSettingsOptions { ApiUrl = expectedUrl });

            // Act
            var value = mockOptions.Object.CurrentValue;

            // Assert
            Assert.IsNotNull(value);
            Assert.AreEqual(expectedUrl, value.ApiUrl);
        }
    }
}