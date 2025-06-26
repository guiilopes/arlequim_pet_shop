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

### ♾ Common-Properties (Propriedade em comum em todas as entidades)

| Propriedade | Tipo       | Descrição             |
|-------------|------------|------------------------|
| `CreatedOn` | `datetime` | Data de criação da informação no sistema |
| `UpdatedOn` | `datetime` | Data de atualização da informação no sistema |
| `DeletedOn` | `datetime` | Data de remoção da informação no sistema |

### 🧾 Sale (Venda)

| Propriedade | Tipo       | Descrição             |
|-------------|------------|------------------------|
| `Id`        | `Guid`     | Identificador da venda |
| `TotalPrice`| `decimal`  | Valor total   |
| `Client`    | `Client`   | Objeto relacionado a entidade cliente  |
| `Products`  | `List<SaleProduct>` | Lista de objetos da venda     |

---

### 📦 Product (Produtos)

| Propriedade     | Tipo     | Descrição          |
|-----------------|----------|--------------------|
| `Id`            | `guid  ` | Identificador único|
| `Barcode`       | `string` | Código de barras   |
| `Name`          | `string` | Nome               |
| `Description`   | `string` | Descrição          |
| `Price`         | `decimal`| Preço unitário     |
| `ExpirationDate`| `dateTime`| Data de validade  |

---

### 📦 User (Usuários)

| Propriedade     | Tipo     | Descrição         |
|-----------------|----------|--------------------|
| `Id`            | `guid`   | Identificador único|
| `Type`          | `enum`   | Nível de acesso    |
| `Name`          | `string` | Nome               |
| `Email`         | `string` | Email             |
| `Password`      | `string` | Senha              |

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
| DTO/Command        | `SaleCreateCommand`, `SaleProductCommandItem` |
| Dependency Injection | `IServiceProvider`                       |
| Testes Unitários   | `NUnit`, `Moq`                              |

---

## 🛠️ Tecnologias

| Tecnologia      | Finalidade              |
|------------------|-------------------------|
| .NET 6+          | Backend principal       |
| DDD (Domain-driven Design) |  Alinhar a lógica de negócio do sistema com sua implementação técnica, tornando o software mais compreensível, coeso e orientado ao domínio real. [Veja mais](https://medium.com/beelabacademy/domain-driven-design-vs-arquitetura-em-camadas-d01455698ec5)     |
| CQRS (Command Query Responsibility Segregation)         |  Separar responsabilidades de leitura (Query) e escrita (Command) para que cada uma evolua de forma independente, com foco em performance, escalabilidade, clareza e manutenção. [Veja mais](https://medium.com/@marcelomg21/cqrs-command-query-responsibility-segregation-em-uma-arquitetura-de-micro-servi%C3%A7os-71dcb687a8a9)      |
| Dapper           | Mapeamento direto de objetos com SQL puro, sem o peso de ORMs tradicionais como Entity Framework. |
| NHibernate       | ORM (Object-Relational Mapping) |
| Docker           | Containerização da aplicação e banco de dados |
| SQL Server       | Banco de dados          |
| NUnit + Moq      | Testes unitários        |
| IConfiguration   | Configuração da aplicação |
| ILogger          | Logs estruturados       |
| SrShut.Data      | Biblioteca e ferramenta auxiliar criada por mim (Guilherme Lopes)       |
| Documentação (Emojis)    | Utilizado o site [GetEmoji](https://getemoji.com/) |

#### 📚 SrShut.Data
📄 Veja o guia completo: [README.md](https://github.com/guiilopes/srshut.data/blob/master/README.md)

> Biblioteca para melhorar e agilizar o desenvolvimento de projetos utilizando DDD, CQRS, EDD, EDA, etc.
> Ainda em fase de documentação;
> Caso queria solicitar a utilização pode me encaminhar uma mensagem, email;

---

## 🔐 Segurança

- 🔐 Autenticação com suporte a JWT 
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

## 📂 Exemplos de usos

```plaintext
🔐 Pré-requisitos

- .NET 6+ rodando localmente
- Banco de dados SQL Server configurado (vide `appsettings.json`)
- API rodando em: `https://localhost:5001` ou `http://localhost:5000`
- Ferramentas recomendadas: Postman ou Swagger
```

##### 1️⃣ Criar usuário
🔸 Endpoint ``POST /api/users``

📥 Requisição
```json
{
  "type": "7897348200994",
  "name": "Guilherme Lopes",
  "email": "guilherme@email.com",
  "password": "********"
}
```
📤 Resposta
```json
{
  "id": "4ab07514-4c0e-4a5c-8d7b-2f67ef38209d"
}
```

##### 2️⃣ Obter token de autenticação 
🔸 Endpoint ``POST /api/login``

📥 Requisição
```json
{
    "email": "guilherme@email.com",
    "password": "********"
}
```
📤 Resposta
```json
{
  "id": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### As etapas abaixo é necessário estar autênticado no sistema.

3️⃣ Adicionar um produto
🔸 Endpoint ``POST /api/products``

📥 Requisição
```json
{
  "barcode": "7897348200994",
  "name": "RAÇÃO PREMIER CÃES ADULTOS PEQUENO PORTE SABOR FRANGO",
  "description": "RAÇÃO PREMIER FÓRMULA PARA CÃES ADULTOS DE RAÇAS PEQUENAS SABOR FRANGO",
  "price": 100.00,
  "expirationDate": "2026-06-26"
}
```
📤 Resposta
```json
{
  "id": "4ab07514-4c0e-4a5c-8d7b-2f67ef38209d"
}
```

4️⃣ Listar Produtos
🔸 Endpoint ``GET /api/products``

>As informações inseridas em cada parâmetro são comentarios do que é possível pesquisar.

📥 Requisição
```json
{
  "text": "Id, Nome ou Descrição",  
  "startExpirationDate": "Utilziado formato em data ISO8601, informando a data inicial de validade do produto",
  "endExpirationDate": "Utilziado formato em data ISO8601, informando a data final de validade do produto",
  "page": "Utilizado em formato numérico (int): Campo utilizado para paginação em telas front-end, indica qual a página deseja buscar, ex: página 1",
  "pageSize": "Utilizado em formato numérico (int): Campo utilizado para paginação em telas front-end, indica o tamanho do resultado de cada página, ex: 20 itens por pagina"
}
```
📤 Exemplo de resposta
```json
{
  "itens": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "string",
      "description": "string",
      "price": 0,
      "expirationDate": "2025-06-26",
      "stockQuantity": 0,
      "createdOn": "2025-06-26",
      "updatedOn": "2025-06-26"
    }
  ],
  "totalCount": 0
}
```

---

5️⃣ Atualizar um determinado produto
🔸 Endpoint ``PUT /api/products/{id}``

📥 Requisição
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",  
  "barcode": "7897348200994",
  "name": "RAÇÃO PREMIER CÃES ADULTOS PEQUENO PORTE SABOR FRANGO",
  "description": "RAÇÃO PREMIER FÓRMULA PARA CÃES ADULTOS DE RAÇAS PEQUENAS SABOR FRANGO",
  "price": 150.00,
  "expirationDate": "2027-06-26"
}
```
📤 Exemplo de resposta
```json
{
  "id": "4ab07514-4c0e-4a5c-8d7b-2f67ef38209d"
}
```

6️⃣ Remover um determinado produto
🔸 Endpoint ``DELETE /api/products/{id}``
> Utilizado para remover um produto (deleção lógica)

📥 Requisição
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```
📤 Exemplo de resposta
```json
{
  "id": "4ab07514-4c0e-4a5c-8d7b-2f67ef38209d"
}
```

7️⃣ Realizar um balanço de estqoue de produtos
🔸 Endpoint ``POST /api/products/stockinventory``

> Utilizado para realizar balanço de estoque via importação de arquivo *.csv contendo os sequintes campos:

```csv
barcode;name;description;price;quantity;expirationdate
7897348200994;RAÇÃO PREMIER FÓRMULA PARA CÃES ADULTOS DE RAÇAS PEQUENAS SABOR FRANGO;RAÇÃO PREMIER FÓRMULA PARA CÃES ADULTOS DE RAÇAS PEQUENAS SABOR FRANGO;100.50;10.0;2025-12-01
```

📥 Requisição
```json
{
  " file": "arquivo.csv"
}
```
📤 Exemplo de resposta
```json
{
  "statusCode": 200
}
```

8️⃣ Listagem de estoque de produtos
🔸 Endpoint ``GET /api/products/stocks``

📥 Requisição
```json
{
  "text": "Id, Nome ou Descrição",  
  "startExpirationDate": "Utilziado formato em data ISO8601, informando a data inicial de validade do produto",
  "endExpirationDate": "Utilziado formato em data ISO8601, informando a data final de validade do produto",
  "page": "Utilizado em formato numérico (int): Campo utilizado para paginação em telas front-end, indica qual a página deseja buscar, ex: página 1",
  "pageSize": "Utilizado em formato numérico (int): Campo utilizado para paginação em telas front-end, indica o tamanho do resultado de cada página, ex: 20 itens por pagina"
}
```
📤 Exemplo de resposta
```json
{
  "itens": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "id": 0,
      "name": "string",
      "description": "string",
      "price": 0,
      "expirationDate": "2025-06-26",
      "quantity": 0,
      "lastDocumentFiscalNumber": "string",
      "createdOn": "2025-06-26",
      "updatedOn": "2025-06-26"
    }
  ],
  "totalCount": 0
}
```

9️⃣ Importação de produtos via XML
🔸 Endpoint ``POST /api/purchases``

> Para realizar a importação é necessário uma nota fiscal real.

📥 Requisição
```json
{
  "file": "arquivo.xml"
}
```
📤 Exemplo de resposta
```json
{
  "statusCode": 200
}
```

1️⃣0️⃣ Listagem de vendas
🔸 Endpoint ``GET /api/sales``

📥 Requisição
```json
{
  "text": "Id, Nome ou Descrição",  
  "page": "Utilizado em formato numérico (int): Campo utilizado para paginação em telas front-end, indica qual a página deseja buscar, ex: página 1",
  "pageSize": "Utilizado em formato numérico (int): Campo utilizado para paginação em telas front-end, indica o tamanho do resultado de cada página, ex: 20 itens por pagina"
}
```
📤 Exemplo de resposta
```json
{
  "itens": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "client": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "string",
        "document": "string"
      },
      "products": [
        {
          "saleId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "name": "string",
          "description": "string",
          "expirationDate": "2025-06-26",
          "quantity": 0,
          "price": 0,
          "discount": 0,
          "netPrice": 0
        }
      ],
      "totalPrice": 0,
      "createdOn": "2025-06-26",
      "updatedOn": "2025-06-26"
    }
  ],
  "totalCount": 0
}
```

1️⃣1️⃣ Realizar uma venda
🔸 Endpoint ``POST /api/sales``

📥 Requisição
```json
{
  "document": "documento do cliente",
  "name": "nome do cliente (caso não preencher, o sistema irá salvar com o documento do cliente)",
  "products": [
    {
      "barcode": "codigo de barras do produto",
      "quantity": 100.00,
      "discount": 25.00
    }
  ]
}
```
📤 Exemplo de resposta
```json
{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

1️⃣1️⃣ Consultar uma determinada  venda
🔸 Endpoint ``GET /api/sales/{id}``

📥 Requisição
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```
📤 Exemplo de resposta
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "client": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "string",
    "document": "string"
  },
  "products": [
    {
      "saleId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "string",
      "description": "string",
      "expirationDate": "2025-06-26",
      "quantity": 0,
      "price": 0,
      "discount": 0,
      "netPrice": 0
    }
  ],
  "totalPrice": 0,
  "createdOn": "2025-06-26",
  "updatedOn": "2025-06-26"
}
```

---

## ✍️ Autor

**Guilherme Henrique Busto Lopes**  
📧 guii-lopes@hotmail.com  
📍 Curitiba – PR  
💼 Analista Desenvolvedor Sênior
