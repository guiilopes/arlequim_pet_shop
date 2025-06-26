
# 📘 ArlequimPetShop – Guia de Uso da API

Este documento fornece um **passo a passo completo** para utilizar os endpoints da API do projeto **ArlequimPetShop**. Os exemplos utilizam JSON via `curl`, Postman ou Swagger UI.

---

## 🔐 Pré-requisitos

- .NET 6+ rodando localmente
- Banco de dados SQL Server configurado (vide `appsettings.json`)
- API rodando em: `https://localhost:5001` ou `http://localhost:5000`
- Ferramentas recomendadas: Postman, Swagger ou Insomnia

---

## 1️⃣ Adicionar um produto

### 🔸 Endpoint
``POST /api/products``

### 📥 Requisição
```json
{
  "name": "Guilherme Lopes",
  "document": "12345678900"
}
```

### 📤 Resposta
```json
{
  "id": "4ab07514-4c0e-4a5c-8d7b-2f67ef38209d"
}
```

---

## 2️⃣ Listar Clientes

### 🔸 Endpoint
``GET /api/clients``

### 💡 Passo
Execute para listar todos os clientes cadastrados.

### 📤 Exemplo de resposta
```json
[
  {
    "id": "4ab07514-4c0e-4a5c-8d7b-2f67ef38209d",
    "name": "Guilherme Lopes",
    "document": "12345678900"
  }
]
```

---

## 3️⃣ Criar Produto (via importação de CSV)

### 🔸 Endpoint
``POST /api/products/stockinventory``

### 📥 Requisição
- Enviar **arquivo CSV** do balanço de estoque como `multipart/form-data`.
- Campo: `file`

### 📤 Resposta esperada
```json
{
  "success": true
}
```

---

## 4️⃣ Listar Produtos

### 🔸 Endpoint
``GET /api/products``

### 📤 Exemplo de resposta
```json
[
  {
    "id": "288068bf-8db2-48ab-95e9-1c09e31e294b",
    "name": "Ração Golden",
    "price": 75.50
  }
]
```

---

## 5️⃣ Criar Venda

### 🔸 Endpoint
``POST /api/sales``

### 📥 Requisição
```json
{
  "document": "12345678900",
  "name": "Guilherme Lopes",
  "products": [
    {
      "barcode": "7891234567890",
      "quantity": 2,
      "discount": 5.00
    }
  ]
}
```

### 📤 Resposta
```json
{
  "id": "f0177f72-e3e6-4b04-9c1d-ae7c22cd59f9"
}
```

---

## 6️⃣ Consultar Venda

### 🔸 Endpoint
``GET /api/sales/{id}``

### 🔍 Requisição
Passe o ID retornado na criação.

### 📤 Exemplo de resposta
```json
{
  "id": "f0177f72-e3e6-4b04-9c1d-ae7c22cd59f9",
  "name": "Guilherme Lopes",
  "document": "12345678900",
  "products": [
    {
      "barcode": "7891234567890",
      "quantity": 2,
      "discount": 5.00,
      "price": 150.00
    }
  ],
  "totalAmount": 295.00
}
```

---

## 7️⃣ Importar Nota Fiscal (NF-e)

### 🔸 Endpoint
``POST /api/documents/import``

### 📥 Requisição
- Enviar arquivo `.xml` da nota como `multipart/form-data`.
- Campo: `file`

### 📤 Exemplo de resposta
```json
{
  "issuer": "Distribuidora XYZ",
  "products": [
    {
      "name": "Ração Golden",
      "quantity": 5,
      "unitPrice": 70.00
    }
  ],
  "totalValue": 350.00
}
```

---

## 🧪 Testando a API

Você pode usar ferramentas como:

- ✅ Swagger: `https://localhost:5001/swagger`
- ✅ Postman: importar os exemplos acima
- ✅ `curl` via terminal:
```bash
curl -X POST https://localhost:5001/api/clients \
  -H "Content-Type: application/json" \
  -d '{"name":"Guilherme Lopes","document":"12345678900"}'
```

---

## 📌 Observações Finais

- Os endpoints seguem o padrão REST.
- Todos os retornos estão em `application/json`.
- Dados sensíveis devem futuramente ser protegidos com autenticação JWT (não implementado por padrão).

---

🛠️ Em caso de dúvidas, consulte o Swagger UI ou envie seu XML de exemplo para análise automática dos produtos.
