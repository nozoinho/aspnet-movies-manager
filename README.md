# 🎬 MovieCatalog – Full-Stack ASP.NET Core + MongoDB Project

## 📌 Overview

MovieCatalog is a secure and modern **full-stack web application** built with **ASP.NET Core MVC**, **MongoDB**, and **JWT Authentication**.  
It allows users to register, log in, and manage their personal movie collection with full CRUD functionality.

---

## 🚀 Features

### 🔐 Authentication & Authorization
- Secure login with **JWT tokens**
- Password hashing and safe user validation
- Role-independent access with per‑user movie ownership
- Movies are filtered by the logged‑in user only

### 🎞️ Movie Management (CRUD)
- Add, edit, view, and delete movies
- Server‑side validation with **ModelState**
- Ownership verification for all operations

### 🗄️ MongoDB Integration
- Custom repository layer
- `MongoClient`, collections, and connection via `appsettings.json`
- User‑scoped queries

### 🧩 Clean Architecture
- Controllers → Services → MongoDB Repository  
- Separation of concerns for easier maintenance
- Strongly typed models with data annotations

### 📘 Swagger API Testing
- Token‑protected endpoints
- Easy API demo for evaluators

---

## 🏗️ Tech Stack

| Layer | Technology |
|------|------------|
| Backend | ASP.NET Core MVC 8 |
| Database | MongoDB Atlas |
| Authentication | JWT (JSON Web Tokens) |
| Frontend | Razor Views, Bootstrap |
| Tools | Swagger UI, Visual Studio Code |

---

## 📁 Project Structure

```
MovieCatalog/
│── Controllers/
│── Models/
│── Services/
│── Repositories/
│── Views/
│── appsettings.json
│── Program.cs
```

---

## 🔧 Setup Instructions

### 1️⃣ Configure MongoDB
In `appsettings.json`:

```json
"MongoSettings": {
  "ConnectionString": "YOUR_MONGODB_ATLAS_CONNECTION_STRING",
  "DatabaseName": "MovieCatalogDB",
  "MoviesCollection": "Movies",
  "UsersCollection": "Users"
}
```

### 2️⃣ Configure JWT
```json
"Jwt": {
  "Key": "YOUR_SECRET_KEY",
  "Issuer": "MovieCatalogIssuer",
  "Audience": "MovieCatalogAudience"
}
```

### 3️⃣ Run the Application
```bash
dotnet restore
dotnet run
```

App runs at:  
➡️ `https://localhost:5140` (HTTPS)  
➡️ `http://localhost:5140` (HTTP)

Swagger available at:  
➡️ `/swagger`

---

## 🧪 How to Test the API (Swagger)

1. Register or log in using the `/api/auth` endpoints.  
2. Copy the returned JWT token.  
3. Click **Authorize** in Swagger.  
4. Paste the token using this format:

```
Bearer YOUR_TOKEN_HERE
```

5. Test Movie CRUD endpoints:
   - `GET /MoviesApi` → shows only the logged‑in user’s movies  
   - `POST /MoviesApi` → adds a new movie  
   - `PUT /MoviesApi/{id}` → updates movie (id must match)  
   - `DELETE /MoviesApi/{id}` → deletes owned movie  

---
