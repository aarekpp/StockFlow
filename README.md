# StockFlow

**Educational project during .NET learning and development**

**StockFlow** is a learning-oriented, layered ERP-style application
designed to simulate a small warehouse and sales management system.

The project is built with **C# and .NET** and focuses on practical
software engineering concepts used in real-world business applications:
clean architecture principles, SOLID, Entity Framework Core, MSSQL, REST
APIs, WPF/MVVM, dependency injection, design patterns, database
transactions, validation, and automated testing.

The main goal is not to create a production-ready ERP system, but to
build a realistic project that demonstrates how different .NET
technologies and architectural concepts work together.

---

## Features

The application covers the core processes of a small warehouse and sales
system:

- Product and category management
- Supplier management
- Customer management
- Sales order creation
- Order status management
- Inventory tracking
- Inventory movement history
- Order items with historical prices
- REST API for application communication
- Desktop WPF client
- Database persistence using Entity Framework Core
- Database migrations
- Business logic separated from API controllers
- Dependency Injection
- Global exception handling
- Transactional order and inventory operations

---

## Architecture

The project architecture is based on Clean / Onion Architecture patterns with Dependency Inversion.

```text
                         ┌──────────────────────┐
                         │    StockFlow.Client  │
                         │        WPF           │
                         │        MVVM          │
                         └──────────┬───────────┘
                                    │ HTTP / JSON
                                    ▼
                         ┌──────────────────────┐
                         │     StockFlow.Api    │
                         │   ASP.NET Core API   │
                         └──────────┬───────────┘
                                    │
                                    ▼
                         ┌──────────────────────┐
                         │ StockFlow.Application│
                         │ Business Logic / DTO │
                         └──────────┬───────────┘
                                    │
                                    ▼
                         ┌──────────────────────┐
                         │    StockFlow.Domain  │
                         │ Entities / Contracts │
                         └──────────────────────┘
                                    ▲
                                    │
                         ┌──────────┴──────────────┐
                         │ StockFlow.Infrastructure│
                         │ EF Core / Repositories  │
                         │ Database / Migrations   │
                         └─────────────────────────┘
```

### Project Structure

```text
StockFlow.sln
│
├── StockFlow.Domain
│   ├── Entities
│   ├── Enums
│   └── Repository Interfaces
│
├── StockFlow.Infrastructure
│   ├── DbContext
│   ├── EF Core Configurations
│   ├── Repositories
│   └── Migrations
│
├── StockFlow.Application
│   ├── DTOs
│   ├── Services
│   ├── Validation
│   └── Service Interfaces
│
├── StockFlow.Api
│   ├── Controllers
│   ├── Middleware
│   ├── Dependency Injection
│   └── Swagger
│
├── StockFlow.Client.Wpf
│   ├── Views
│   ├── ViewModels
│   ├── Models
│   └── API Client
│
└── StockFlow.Tests
    ├── Unit Tests
    └── Integration Tests
```

The **Domain** layer has no dependency on Entity Framework Core.
Infrastructure implements persistence-related interfaces, Application
contains business logic, and the API is responsible for exposing that
functionality through HTTP.

The WPF client communicates exclusively with the API and does not access
the database directly.

---

## Technology Stack

Area Technology

---

Language C#
Backend ASP.NET Core Web API
Framework .NET 10
ORM Entity Framework Core
Database Microsoft SQL Server / LocalDB
Desktop Client WPF
Desktop Architecture MVVM
API Communication HTTP + JSON
JSON System.Text.Json
API Documentation Swagger / Swashbuckle
Dependency Injection Microsoft.Extensions.DependencyInjection
Testing xUnit + Moq
Logging Serilog
Authentication JWT
Version Control Git + GitHub

---

## Domain Model

The database models the main entities involved in warehouse and sales
operations.

```text
Category
   │
   └─────── 1:N ─────── Product
                         │
                         │ N:N
                         │
                         ▼
                    Supplier
                (ProductSupplier)

Customer
   │
   └─────── 1:N ─────── Order
                         │
                         │ N:N
                         │
                         ▼
                       Product
                  (OrderItem)

Product
   │
   └─────── 1:N ─────── StockMovement

Order
   │
   └─────── 1:N ─────── StockMovement
```

### Main Entities

- **Category**
- **Product**
- **Supplier**
- **ProductSupplier**
- **Customer**
- **Order**
- **OrderItem**
- **StockMovement**
- **User**

Two many-to-many relationships intentionally contain additional data.

### Product ↔ Supplier

The relationship stores information such as:

- Purchase price
- Delivery time in days

This allows the same product to have different purchasing conditions
depending on the supplier.

### Order ↔ Product

The order item stores:

- Quantity
- Product
- Price at the moment of ordering

The historical price is stored directly on the order item so that
changing the current product price does not modify the historical value
of an existing order.

---

## Core Business Flow

One of the main business processes implemented by the project is order
processing.

```text
Customer
   │
   ▼
Create Order
   │
   ├── Validate products
   ├── Check stock availability
   ├── Store order items
   └── Store historical prices
   │
   ▼
Order Processing
   │
   ▼
Order Completed
   │
   ├── Create stock movement
   └── Decrease inventory
```

---

## API

The API provides endpoints for the main business entities.

Examples include:

```http
GET    /categories
GET    /categories/{id}
POST   /categories
PUT    /categories/{id}
DELETE /categories/{id}
```

```http
GET    /products
GET    /products/{id}
POST   /products
PUT    /products/{id}
DELETE /products/{id}
```

Order-related operations include:

```http
POST  /orders
PATCH /orders/{id}/status
```

Product--supplier relationships are managed through dedicated endpoints:

```http
POST /products/{id}/suppliers
```

---

## Database

The application uses **Microsoft SQL Server**, with **LocalDB** intended
for development.

Entity Framework Core follows the Code First approach:

```text
Domain Entities
      │
      ▼
EF Core Fluent Configuration
      │
      ▼
DbContext
      │
      ▼
Migration
      │
      ▼
Microsoft SQL Server
```

---

## WPF Client

The WPF application acts as an external client of the REST API.

It does not communicate directly with the database:

```text
WPF
 │
 │ HTTP / JSON
 ▼
ASP.NET Core API
 │
 ▼
Application
 │
 ▼
Infrastructure
 │
 ▼
MSSQL
```

The client is structured according to the **MVVM** pattern and is
intended to provide screens for:

- Products
- Customers
- Suppliers
- Order creation
- Order items / cart
- Basic error handling

---

## Project Goals

This project is primarily a practical learning and portfolio project.

The main objectives are to gain hands-on experience with:

- C# and .NET development
- Object-Oriented Programming
- SOLID principles
- Layered architecture
- Design patterns
- Entity Framework Core
- MSSQL
- REST APIs
- Dependency Injection
- DTOs and validation
- Asynchronous programming
- LINQ
- WPF and MVVM
- Git and GitHub
- Unit and integration testing
- Business logic implementation

The scope is intentionally limited. The project does **not** aim to
become a complete ERP product and does not currently include features
such as invoicing, payments, or multiple warehouses.

---

## Status

**Development in progress.**

The project is being developed incrementally, with each stage
introducing new .NET concepts and gradually expanding the application
from a domain model into a complete API + desktop client solution.
