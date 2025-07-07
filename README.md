# Library Catalog - Monolith

**Language / Idioma:** [🇺🇸 English](#) | [🇧🇷 Português](README-pt-br.md)

This is a monolith project that contains both the backend and frontend of the Library Catalog application.

## Project Structure

```
library-catalog/
├── library-backend/    # .NET Core Web API
├── library-frontend/   # Angular Application
└── README.md
```

## Technologies Used

### Backend (library-backend)
- .NET Core 8+
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server

### Frontend (library-frontend)
- Angular
- TypeScript
- HTML/CSS

## How to Run

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

## Functional Requirements

- **User Account Management**: Users can create accounts with name, date of birth, email/login, and password
- **Password Reset**: Users can reset their password via email
- **Authentication**: Login system using email and password
- **Book Catalog**: Personal catalog of books for each authenticated user
- **Book Registration**: Register books with title, ISBN, genre (select), author, publisher (select), synopsis (max 5000 characters), and book photo (IFormFile)
- **Book Search**: Search books by title, ISBN, author, publisher, or genre (partial or complete matches)
- **Book Management**: List, update, and delete registered books
- **PDF Reports**: Generate PDF reports with all registered books per logged user

## Non-Functional Requirements

- **Data Validation**: Proper validation for all input fields
- **Usability**: Good usability and ease of operation
- **Architecture**: Layered architecture with Angular frontend and .NET Core Web API backend
- **Database**: Entity Framework Core (Code First) with SQL Server
- **Framework**: .NET Core 8+ / C#

## Architecture

The project follows a layered architecture with clear separation of responsibilities:

- **Frontend (Angular)**: Responsive user interface consuming REST API
- **Backend (Web API)**: RESTful API with business logic and data persistence
- **Database (SQL Server)**: Data storage managed by Entity Framework Core
