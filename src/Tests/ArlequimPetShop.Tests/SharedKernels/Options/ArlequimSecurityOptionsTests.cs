using ArlequimPetShop.SharedKernel.Options;
using Microsoft.Extensions.Options;
using Moq;

namespace ArlequimPetShop.Tests.SharedKernels.Options
{
    /// <summary>
    /// Testes para a classe de configuração ArlequimSecurityOptions.
    /// </summary>
    [TestFixture]
    public class ArlequimSecurityOptionsTests
    {
        [Test]
        public void OptionsMonitor_ShouldReturnConfiguredSecret()
        {
            // Arrange
            var expectedSecret = "MinhaChaveSecreta123";
            var mockOptions = new Mock<IOptionsMonitor<ArlequimSecurityOptions>>();
            mockOptions.Setup(o => o.CurrentValue).Returns(new ArlequimSecurityOptions { Secret = expectedSecret });

            // Act
            var optionsValue = mockOptions.Object.CurrentValue;

            // Assert
            Assert.IsNotNull(optionsValue);
            Assert.AreEqual(expectedSecret, optionsValue.Secret);
        }
    }
}