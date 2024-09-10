# Minimal API - Projeto de Exemplo

Este repositório contém uma **Minimal API** desenvolvida em **.NET 8** para fins educativos, com autenticação JWT, utilização de Entity Framework Core para gerenciamento de dados e testes de integração com **MSTest**. A API permite a criação e gerenciamento de administradores e veículos em um sistema de estacionamento.

## Índice

- [Recursos da API](#recursos-da-api)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Requisitos](#requisitos)
- [Instalação](#instalação)
- [Testes](#testes)
- [Exemplos de Uso](#exemplos-de-uso)
- [Contribuindo](#contribuindo)

## Recursos da API

A Minimal API oferece as seguintes funcionalidades:

1. **Autenticação JWT**: Rotas protegidas por autenticação.
2. **Gerenciamento de Administradores**: Criação, consulta, edição e exclusão de administradores.
3. **Gerenciamento de Veículos**: Controle de entrada e saída de veículos.

## Tecnologias Utilizadas

- **.NET 8**
- **Minimal API**
- **SQL Server**
- **Entity Framework Core** (com suporte a banco de dados em memória)
- **JWT (JSON Web Token)** para autenticação
- **MSTest**  para testes de unitários, de persistência e de integração
- **Swagger** para documentação de rotas

## Requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- [Postman](https://www.postman.com/downloads/) (opcional)

## Instalação

1. Clone o repositório:

    ```bash
    git clone https://github.com/gabrielctavares/minimal-api.git
    ```

2. Acesse a pasta do projeto:

    ```bash
    cd minimal-api
    ```

3. Restaure os pacotes NuGet:

    ```bash
    dotnet restore
    ```

4. Configure a string de conexão no arquivo `appsettings.json` para seu banco de dados SQL Server:

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=MinimalApiDb;Trusted_Connection=True;"
    }
    ```

5. Execute as migrações do Entity Framework para criar o banco de dados:

    ```bash
    dotnet ef database update
    ```

6. Execute a aplicação:

    ```bash
    dotnet run
    ```

A API estará disponível em `https://localhost:5001`.

---

## Testes

Este projeto utiliza **MSTest** para executar testes que validam as funcionalidades da API. Os testes incluem cenários como login e acesso a rotas protegidas por JWT.

### Estrutura dos Testes

- **Dominio/Entidades**: Realiza testes unitários relacionados as Entidades.
- **Dominio/Services**: Realiza testes de persistência relacionados as Services.
- **Requests**: Realiza testes de integração.

### Executando os Testes

Para rodar os testes, execute o seguinte comando:

```bash
dotnet test
```

## Exemplos de Uso

### Fazer Login

**Requisição:**

```http
POST /administradores/login
Content-Type: application/json

{
  "Email": "Gabriel",
  "senha": "1234"
}
```
**Resposta:**
```json
{
  "email": "email@....",
  "token": "eyJhbGciOiJI..."
}
```

### Listar Veículos

**Requisição:**

```http
GET /api/veiculos
Authorization: Bearer {token}
```

**Resposta:**

```json
[
  {
    "placa": "ABC1234",
    "modelo": "Sedan"
  },
  {
    "placa": "XYZ5678",
    "modelo": "Hatchback"
  }
]
```

## Contribuindo

1. **Faça um fork** do repositório.

2. **Crie uma nova branch** para suas mudanças:

    ```bash
    git checkout -b minha-feature
    ```

3. **Commit suas alterações**:

    ```bash
    git commit -m 'Adiciona nova feature'
    ```

4. **Faça push para a branch**:

    ```bash
    git push origin minha-feature
    ```

5. **Envie um Pull Request** para o repositório principal.

