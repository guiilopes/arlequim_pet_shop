using ArlequimPetShop.Domain.Clients;
using SrShut.Common.Exceptions;

namespace ArlequimPetShop.Tests.Domains
{
    [TestFixture]
    public class ClientTests
    {
        [Test]
        public void Constructor_ShouldSetName_WhenNameIsProvided()
        {
            var id = Guid.NewGuid();
            var client = new Client(id, "João", "12345678900");

            Assert.AreEqual(id, client.Id);
            Assert.AreEqual("João", client.Name);
            Assert.AreEqual("12345678900", client.Document);
            Assert.That(client.CreatedOn, Is.EqualTo(client.UpdatedOn).Within(TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void Constructor_ShouldSetNameToDocument_WhenNameIsNull()
        {
            var id = Guid.NewGuid();
            var client = new Client(id, null, "99999999999");

            Assert.AreEqual("99999999999", client.Name);
        }

        [Test]
        public void Update_ShouldChangeName_AndUpdateTimestamp()
        {
            var client = new Client(Guid.NewGuid(), "Maria", "123");
            var originalUpdate = client.UpdatedOn;

            Thread.Sleep(10);
            client.Update("Maria Souza");

            Assert.AreEqual("Maria Souza", client.Name);
            Assert.That(client.UpdatedOn, Is.GreaterThan(originalUpdate));
        }

        [Test]
        public void ShouldThrow_WhenDocumentIsNull()
        {
            Assert.Throws<BusinessException>(() =>
            {
                var client = new Client(Guid.NewGuid(), "Sem Documento", null);
            });
        }
    }
}