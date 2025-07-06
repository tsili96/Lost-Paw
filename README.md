# Lost Paw

**A Web Application for Lost, Found and Adoption Animal Posts using ASP.NET Core MVC**

## Overview

**Lost Paw** is a feature-rich web application developed using **.NET 6**, designed to help users report lost pets, share found animals and list pets available for adoption. It supports real-time messaging, interactive map-based location services and user account management.

Built with modern web technologies including **ASP.NET Core MVC**, **Entity Framework Core**, **ASP.NET Identity**, **SignalR**, and **Google Maps API**, the application delivers a robust platform for connecting pet owners, finders, and adopters.

> 🔒 The application uses secure user authentication. Guests can explore public content, but must register to interact fully.

---

## Features

### User Roles

- **Guest**:
  - View posts
  - View user profiles
  - Cannot create posts or send messages
  - Map-based vet search

- **Registered User**:
  - Full access to post creation/editing
  - Real-time chat with other users
  - Profile management
  - Map-based vet search

### Core Functionalities

- **Authentication**:
  - Register, login, logout
  - Role-based access (Guest vs Registered)

- **Posts**:
  - Create, update, and delete posts for:
    - Lost pets
    - Found animals
    - Adoption offers
  - Search posts by title or username
  -Filter posts by post type or date 

- **User Profiles**:
  - View/edit your own profile and settings
  - View other users' profiles

- **Real-Time Chat**:
  - Built with SignalR
  - One-to-one messaging between registered users

- **Interactive Maps**:
  - Automatically detect and show user’s location
  - Option to manually update location
  - Display nearest veterinary clinics via Google Maps API  
  > Note: The API key is required and **not provided in the public repository** for security reasons

---

## Technology Stack

| Area          | Technology                     |
|---------------|--------------------------------|
| Framework     | ASP.NET Core MVC (.NET 6)      |
| ORM           | Entity Framework Core          |
| Auth          | ASP.NET Identity               |
| DB            | SQL Server                     |
| Real-time     | SignalR                        |
| Maps          | Google Maps JavaScript API     |
| Frontend      | Razor Views, Bootstrap, JS     |

---

## Project Structure

The project follows a clean ASP.NET Core MVC architecture with clearly separated concerns:

- Controllers: 
Handles HTTP requests and returns views or data. Each controller corresponds to a major feature (e.g., PostsController, AccountController, ChatController).

- Models: 
Core domain entities such as User, Post, Pet, etc.

- ViewModels:
Data transfer objects (DTOs) used to pass structured data from controllers to views, keeping models clean.

- Views:
Razor .cshtml files organized into folders by feature (e.g., Posts, Account, Chat, Shared). These define what the user sees.

- Data:
Contains the Entity Framework Core database context and related configurations:

- Migrations: Auto-generated EF migration files

- EntityTypeConfigurations: Fluent API classes used to configure entity relationships and constraints

- Hubs: 
SignalR hubs for real-time messaging features.

- Services: 
Custom service classes that contain business logic (e.g., chat services, location services).

- wwwroot:
Static assets like CSS, JavaScript, images, and third-party libraries.

- appsettings.json:
Stores configuration such as database connection strings and external API settings.

⚠️ Note: Do not commit sensitive values like your Google Maps API key to version control.

---

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- A valid **Google Maps API key**

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/lost-paw.git
   cd lost-paw
2. **Configure the database**
   ```bash
   "ConnectionStrings": { "DefaultConnection": "Server=YOUR_SERVER;Database=LostPawDB;Trusted_Connection=True;"}
3. **Add Google Maps API Key**
- Store your API key securely (e.g., appsettings.json or secrets manager)
- Ensure it is not committed to the public repo
4. **Apply EF Migrations**
- Open Package Manager Console in Visual Studio:
    ```bash
    Update-Database
5. **Run the application**
- Press F5 or click Start Debugging in Visual Studio