# 🎬 ASP.NET Movies Manager

<p align="center">
  <a href="https://aspnet-movies-manager.onrender.com">
    <img src="https://img.shields.io/badge/LIVE%20DEMO-OPEN%20APPLICATION-2ea44f?style=for-the-badge&logo=render&logoColor=white" alt="Open Live Demo">
  </a>
</p>

<p align="center">
  <strong>Secure ASP.NET Core movie management application with JWT authentication, MongoDB Atlas, Razor Views, REST endpoints, Docker, and cloud deployment</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white" alt=".NET 9">
  <img src="https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?logo=dotnet&logoColor=white" alt="ASP.NET Core MVC">
  <img src="https://img.shields.io/badge/MongoDB-Atlas-47A248?logo=mongodb&logoColor=white" alt="MongoDB Atlas">
  <img src="https://img.shields.io/badge/JWT-Authentication-000000?logo=jsonwebtokens&logoColor=white" alt="JWT Authentication">
  <img src="https://img.shields.io/badge/Docker-Containerized-2496ED?logo=docker&logoColor=white" alt="Docker">
  <img src="https://img.shields.io/badge/Render-Deployed-000000?logo=render&logoColor=white" alt="Render">
</p>

<p align="center">
  <a href="https://aspnet-movies-manager.onrender.com"><strong>https://aspnet-movies-manager.onrender.com</strong></a>
</p>

---

## Overview

**ASP.NET Movies Manager** is a full-stack ASP.NET Core application for securely managing personal movie collections.

The application combines an MVC web interface with REST endpoints, JWT-based authentication, MongoDB Atlas persistence, session-backed authentication flow, Swagger/OpenAPI support, automated tests, Docker containerization, and cloud deployment on Render.

Unauthenticated visitors are redirected to the login page, while authenticated users can access their own movie catalog and perform movie management operations.

### Highlights

- JWT-based authentication and protected access
- User-scoped movie ownership
- Complete movie CRUD workflow
- ASP.NET Core MVC with Razor Views
- REST API endpoints for movie operations
- MongoDB Atlas persistence
- Password hashing with BCrypt
- Swagger/OpenAPI support with Bearer authentication
- Server-side validation
- Session-based JWT retrieval for MVC requests
- Automated tests with xUnit, Moq, and Mongo2Go
- Dockerized production deployment
- Environment-based secret management
- Public deployment on Render

---

## Tech Stack

| Area | Technologies |
| --- | --- |
| **Framework** | .NET 9, ASP.NET Core MVC |
| **Language** | C# |
| **Frontend** | Razor Views, Bootstrap, HTML, CSS, JavaScript |
| **Primary Persistence** | MongoDB Atlas, MongoDB.Driver |
| **Authentication** | JWT Bearer, BCrypt.Net |
| **API Documentation** | Swagger / Swashbuckle |
| **Additional Data Access** | Entity Framework Core 9, SQLite |
| **Testing** | xUnit, Moq, Mongo2Go |
| **Containerization** | Docker |
| **Deployment** | Render |
| **Development** | Visual Studio Code / .NET CLI |

---

## Architecture

```text
                         Browser
                            |
                            v
                +-----------------------+
                |       Render          |
                |  Docker Web Service   |
                +-----------+-----------+
                            |
                            v
                +-----------------------+
                |   ASP.NET Core 9      |
                | MVC + REST Endpoints  |
                +-----------+-----------+
                            |
                  +---------+---------+
                  |                   |
                  v                   v
          +---------------+   +---------------+
          | Controllers   |   | Authentication|
          | MVC + API     |   | JWT + Session |
          +-------+-------+   +---------------+
                  |
                  v
          +---------------+
          | Services      |
          | Movie / User  |
          | JWT           |
          +-------+-------+
                  |
                  v
          +---------------+
          | MongoDB Atlas |
          +---------------+
```

The application separates HTTP handling, business logic, authentication, persistence, and presentation responsibilities while supporting both MVC pages and API access.

---

## Core Features

### Authentication and Authorization

The application protects movie-management functionality using JWT authentication.

Key behaviors include:

- JWT token generation
- JWT issuer and audience validation
- Token lifetime validation
- Signing-key validation
- Session-backed JWT retrieval for MVC requests
- Redirecting unauthenticated users to the login flow
- Per-user movie ownership

### Movie Management

Authenticated users can manage movie records through standard CRUD operations:

- View movies
- Create movies
- View movie details
- Edit movies
- Delete movies

Movie access is scoped to the authenticated user.

### REST API

The project includes API endpoints for working with movies programmatically in addition to the MVC interface.

Swagger/OpenAPI is configured with Bearer token support for testing authenticated API operations.

### MongoDB Atlas

The primary application persistence layer uses MongoDB Atlas through `MongoDB.Driver`.

Application services use MongoDB collections for movie and user data.

### Validation

ASP.NET Core model validation is used to validate incoming data before processing and persistence.

---

## Testing

The repository includes automated tests for key services.

### Test Suite

- `JwtServiceTests.cs`
- `MovieServiceTests.cs`
- `UserServiceTests.cs`

Testing technologies include:

- **xUnit**
- **Moq**
- **Mongo2Go**
- **Microsoft.NET.Test.Sdk**

Run the test suite with:

```bash
dotnet test
```

---

## Project Structure

```text
aspnet-movies-manager/
├── Controllers/
├── Migrations/
├── Models/
├── MovieCatalog.Tests/
│   ├── JwtServiceTests.cs
│   ├── MovieServiceTests.cs
│   └── UserServiceTests.cs
├── Properties/
├── Services/
├── Views/
├── wwwroot/
├── .dockerignore
├── .gitignore
├── Dockerfile
├── MovieCatalog.csproj
├── MovieCatalog.sln
├── Program.cs
├── appsettings.Development.json
├── appsettings.json
└── README.md
```

---

## Configuration

Sensitive production values are not stored directly in the public repository.

The application expects configuration for MongoDB and JWT authentication.

### Required Secrets

```text
MongoSettings:ConnectionString
Jwt:Key
```

For environment variables, ASP.NET Core uses double underscores for nested configuration keys:

```text
MongoSettings__ConnectionString
Jwt__Key
```

Non-sensitive settings such as database name, collection names, JWT issuer, and JWT audience can remain in `appsettings.json`.

---

## Local Development

### Requirements

- .NET 9 SDK
- MongoDB Atlas access or a compatible MongoDB instance
- Git

### 1. Clone the repository

```bash
git clone https://github.com/nozoinho/aspnet-movies-manager.git
cd aspnet-movies-manager
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Configure local secrets

Initialize .NET User Secrets if needed:

```bash
dotnet user-secrets init
```

Set the MongoDB connection string:

```bash
dotnet user-secrets set "MongoSettings:ConnectionString" "<your MongoDB connection string>"
```

Set the JWT signing key:

```bash
dotnet user-secrets set "Jwt:Key" "<your JWT signing key>"
```

### 4. Run the application

```bash
dotnet run
```

Use the local URL shown by ASP.NET Core in the terminal.

### 5. Run tests

```bash
dotnet test
```

---

## Run with Docker

Build the production image:

```bash
docker build -t aspnet-movies-manager .
```

Create a local environment file such as `.env.docker`:

```text
MongoSettings__ConnectionString=<your MongoDB connection string>
Jwt__Key=<your JWT signing key>
```

Run the container:

```bash
docker run --rm --env-file .env.docker -p 8080:8080 aspnet-movies-manager
```

Then open:

```text
http://localhost:8080
```

Local environment files containing secrets should remain excluded from version control.

---

## Swagger / API Testing

Swagger is configured in the application and supports JWT Bearer authentication.

When the application is running, Swagger UI is available at:

```text
/swagger
```

Typical workflow:

1. Authenticate and obtain a JWT.
2. Open Swagger UI.
3. Use **Authorize**.
4. Supply the Bearer token.
5. Test the protected movie API endpoints.

---

## Production Deployment

The application is deployed as a Docker-based ASP.NET Core web service on Render.

### Live Application

**[Open ASP.NET Movies Manager](https://aspnet-movies-manager.onrender.com)**

### Deployment Flow

```text
GitHub main branch
        |
        v
Render
        |
        v
Docker Build
        |
        v
ASP.NET Core 9 Web Service
        |
        v
MongoDB Atlas
```

Production secrets are configured directly in Render using:

```text
MongoSettings__ConnectionString
Jwt__Key
```

The Docker container binds ASP.NET Core to the port supplied by the hosting environment.

> The hosting service may require a short startup period after inactivity.

---

## Security Practices

- MongoDB credentials are externalized from source control
- JWT signing keys are supplied through secure configuration
- .NET User Secrets are used for local development
- Production secrets are configured directly in Render
- Passwords are hashed using BCrypt
- JWT issuer, audience, lifetime, and signing key are validated
- Movie access is scoped to the authenticated user
- Local environment files are excluded from Git
- Docker build output and local development artifacts are excluded from version control

---

## Future Improvements

- Role-based authorization
- Refresh tokens and token rotation
- Account registration and password-reset UX improvements
- More comprehensive integration and controller tests
- Search, filtering, sorting, and pagination
- CI/CD validation before production deployment
- Health checks and application monitoring
- Improved API error responses and centralized exception handling

---

## Author

**Fernando Ferreyra**

[GitHub Profile](https://github.com/nozoinho)

---

<p align="center">
  <strong>Built with .NET 9, ASP.NET Core, MongoDB Atlas, JWT, Docker, and Render.</strong>
</p>
