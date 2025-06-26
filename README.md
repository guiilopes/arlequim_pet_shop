# 🐶 ArlequimPetShop – Sistema de Gestão Petshop

Sistema modular de gerenciamento de vendas, produtos e clientes voltado ao segmento petshop, utilizando padrões modernos de arquitetura como **DDD**, **CQRS**, **Clean Code** e **NHibernate**.

---

## 🧱 Arquitetura em Camadas

```plaintext
Presentation (Controllers / APIs)
│
├── Application (Commands / Queries / Services)
│
├── Domain (Entities / Aggregates / Interfaces)
│
├── Infrastructure (Databases / Repositories NHibernate / Messaging)
│
└── SharedKernel (Utilitários, ValueObjects, Exceptions)
```

---

## 📁 Namespaces e Responsabilidades

| Namespace                         | Descrição                                                                 |
|----------------------------------|---------------------------------------------------------------------------|
| `ArlequimPetShop.Domain.*`       | Entidades como `Sale`, `Product`, interfaces como `ISaleRepository`      |
| `ArlequimPetShop.Application.*`  | Serviços de aplicação, handlers de comandos, regras de orquestração      |
| `ArlequimPetShop.Infrastructure.*` | Repositórios NHibernate, acesso a banco, filas, etc.                     |
| `ArlequimPetShop.API`            | Endpoints REST (ASP.NET Core Controllers)                                 |
| `ArlequimPetShop.SharedKernel`   | Utilitários, validações comuns, enums, datas                              |
| `SrShut.Nhibernate`              | Infraestrutura NHibernate e Unit of Work                                  |
| `SrShut.Common`                  | Helpers genéricos (Datas, Strings, XML, Arquivos)                         |

---

## 🛒 Entidades Principais

### 🧾 Sale (Venda)

| Propriedade | Tipo       | Descrição             |
|-------------|------------|------------------------|
| `Id`        | `Guid`     | Identificador da venda |
| `Document`  | `string`   | Documento do cliente   |
| `Name`      | `string`   | Nome do cliente        |
| `Products`  | `List<SaleProduct>` | Itens da venda     |

---

### 📦 SaleProduct

| Propriedade | Tipo     | Descrição                     |
|-------------|----------|-------------------------------|
| `Barcode`   | `string` | Código de barras do produto   |
| `Quantity`  | `decimal`| Quantidade vendida            |
| `Discount`  | `decimal`| Desconto aplicado             |

---

### 🐕 Product

| Propriedade     | Tipo     | Descrição         |
|-----------------|----------|--------------------|
| `Barcode`       | `string` | Identificador único|
| `Name`          | `string` | Nome do produto    |
| `StockQuantity` | `decimal`| Estoque atual      |
| `Price`         | `decimal`| Preço unitário     |

---

## 🚀 Fluxo da Venda

```plaintext
[POST] /api/sale
│
└── Controller
    └── SaleCreateCommand
        └── CommandHandler
            └── Domain Service
                └── SaleNHRpository (Infrastructure)
                    └── NHibernate Session
```

---

## 🔧 Padrões Utilizados

| Padrão             | Implementação                               |
|--------------------|----------------------------------------------|
| Repository         | `ISaleRepository`, `SaleNHRpository`        |
| Unit of Work       | `IUnitOfWorkFactory<ISession>`             |
| Domain Service     | Regras complexas fora da entidade           |
| Event Sourcing     | `EventBusRepository<T>`                    |
| DTO/Command        | `SaleCreateCommand`, `SaleProductCommandItem` |
| Dependency Injection | `IServiceProvider`                       |
| Testes Unitários   | `NUnit`, `Moq`                              |

---

## 🧪 Testes Recomendados

| Tipo         | O que testar                                   |
|--------------|------------------------------------------------|
| Comandos     | Validação de regras, fluxo do handler          |
| Domínio      | Regras dentro de entidades ou serviços de domínio |
| Repositórios | Acesso ao banco via mocks                      |
| API          | Testes de integração com `WebApplicationFactory`|

---

## 🛠️ Tecnologias

| Tecnologia      | Finalidade              |
|------------------|-------------------------|
| .NET 6+          | Backend principal       |
| NHibernate       | ORM (Object-Relational Mapping) |
| Docker           | Containerização da aplicação e banco de dados |
| SQL Server       | Banco de dados          |
| NUnit + Moq      | Testes unitários        |
| IConfiguration   | Configuração da aplicação |
| ILogger          | Logs estruturados       |

---

## 🔐 Segurança

- 🔐 Autenticação com suporte a JWT (opcional)
- ✅ Validação de entrada via DataAnnotations ou FluentValidation
- 🧱 Separação de responsabilidades com arquitetura em camadas

---

## 🗃️ Banco de Dados Relacional

```plaintext
Sale (1) ─────── (N) SaleProduct
│                      │
└────> Client (1) ─────┘
```

---

## 💡 Exemplo de Repositório

```csharp
public class SaleNHRpository : EventBusRepository<Sale>, ISaleRepository
{
    public SaleNHRpository(IConfiguration configuration, IUnitOfWorkFactory<ISession> sessionManager, IServiceProvider serviceProvider): base(configuration, sessionManager, serviceProvider)
    {
    }
}
```

---

## 📂 Estrutura de Projeto

```plaintext
/ArlequimPetShop.API
/ArlequimPetShop.Application
/ArlequimPetShop.Domain
/ArlequimPetShop.Infrastructure
/ArlequimPetShop.SharedKernel
/SrShut.Common
/SrShut.Nhibernate
/tests/...
```

---

## ✍️ Autor

**Guilherme Henrique Busto Lopes**  
📧 guii-lopes@hotmail.com  
📍 Curitiba – PR  
💼 Analista Desenvolvedor Sênior
