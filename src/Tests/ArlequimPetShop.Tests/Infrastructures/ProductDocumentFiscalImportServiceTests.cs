using ArlequimPetShop.Domain.Products;
using ArlequimPetShop.Domain.Products.Services;
using ArlequimPetShop.Infrastructure.Services.Products;
using Moq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ArlequimPetShop.Tests.Infrastructures;

/// <summary>
/// Testes para o serviço de importação fiscal de produtos a partir de XML da NF-e.
/// </summary>
[TestFixture]
public class ProductDocumentFiscalImportServiceTests
{
    private Mock<IProductRepository>? _productRepositoryMock;
    private ProductDocumentFiscalImportService? _service;

    [SetUp]
    public void SetUp()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _service = new ProductDocumentFiscalImportService(_productRepositoryMock.Object);
    }

    /// <summary>
    /// Garante que produtos novos são adicionados e atualizados corretamente com histórico e estoque.
    /// </summary>
    [Test]
    public async Task Execute_ShouldImportProductsAndUpdateRepository()
    {
        var dto = new ProductDocumentFiscalDto
        {
            NFe = new NFe
            {
                InfNFe = new InfNFe
                {
                    Id = "12345",
                    Det =
                    [
                        new Det
                        {
                            Prod = new Prod
                            {
                                BarCode = "123456789",
                                Name = "Ração Premium",
                                Price = 100.0m
                            }
                        },
                        new Det
                        {
                            Prod = new Prod
                            {
                                BarCode = "123456789",
                                Name = "Ração Premium",
                                Price = 100.0m
                            }
                        }
                    ]
                }
            }
        };

        var xml = Serialize(dto);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        _productRepositoryMock?.Setup(r => r.GetAsyncByBarcode("123456789"))
                               .ReturnsAsync((Product)null!);

        await _service?.Execute(stream);

        _productRepositoryMock?.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        _productRepositoryMock?.Verify(r => r.UpdateAsync(It.Is<Product>(p => p.Barcode == "123456789" 
                                                                           && p.Histories.Any() 
                                                                           && p.Stocks.Count() > 0)), Times.Once);
    }

    private static string Serialize(ProductDocumentFiscalDto dto)
    {
        var xmlSerializer = new XmlSerializer(typeof(ProductDocumentFiscalDto));
        var xmlSettings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true),
            Indent = true,
            OmitXmlDeclaration = false
        };

        using var stringWriter = new StringWriterWithEncoding(Encoding.UTF8);
        using var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings);

        xmlSerializer.Serialize(xmlWriter, dto);
        return stringWriter.ToString();
    }

    // Classe auxiliar para forçar Encoding no StringWriter
    public class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding _encoding;

        public StringWriterWithEncoding(Encoding encoding) => _encoding = encoding;

        public override Encoding Encoding => _encoding;
    }
}