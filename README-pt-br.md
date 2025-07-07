# Library Catalog - Monolito

**Language / Idioma:** [🇺🇸 English](README.md) | [🇧🇷 Português](#)

Este é um projeto monolito que contém tanto o backend quanto o frontend da aplicação Library Catalog.

## Estrutura do Projeto

```
library-catalog/
├── library-backend/    # Web API .NET Core
├── library-frontend/   # Aplicação Angular
└── README.md
```

## Tecnologias Utilizadas

### Backend (library-backend)
- .NET Core 8+
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server

### Frontend (library-frontend)
- Angular
- TypeScript
- HTML/CSS

## Como Executar

### Backend
```bash
cd library-backend
dotnet run
```

### Frontend
```bash
cd library-frontend
npm install
ng serve
```

## Requisitos Funcionais

- **Gestão de Contas de Usuário**: Usuários podem criar contas com nome, data de nascimento, e-mail/login e senha
- **Reset de Senha**: Usuários podem resetar sua senha via e-mail
- **Autenticação**: Sistema de login usando e-mail e senha
- **Catálogo de Livros**: Catálogo pessoal de livros para cada usuário autenticado
- **Cadastro de Livros**: Registrar livros com título, ISBN, gênero (select), autor, editora (select), sinopse (máx 5000 caracteres) e foto do livro (IFormFile)
- **Busca de Livros**: Buscar livros por título, ISBN, autor, editora ou gênero (correspondência parcial ou completa)
- **Gestão de Livros**: Listar, atualizar e excluir livros cadastrados
- **Relatórios PDF**: Gerar relatórios em PDF com todos os livros cadastrados por usuário logado

## Requisitos Não Funcionais

- **Validação de Dados**: Validação adequada para todos os campos de entrada
- **Usabilidade**: Boa usabilidade e facilidade de operação
- **Arquitetura**: Arquitetura em camadas com frontend Angular e backend .NET Core Web API
- **Banco de Dados**: Entity Framework Core (Code First) com SQL Server
- **Framework**: .NET Core 8+ / C#

## Arquitetura

O projeto segue uma arquitetura em camadas com separação clara de responsabilidades:

- **Frontend (Angular)**: Interface do usuário responsiva consumindo API REST
- **Backend (Web API)**: API RESTful com lógica de negócio e persistência de dados
- **Banco de Dados (SQL Server)**: Armazenamento de dados gerenciado pelo Entity Framework Core
