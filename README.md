# Ordering System

## Overview
Ordering System is a web-based application designed to facilitate seamless order management. It follows the Onion Architecture to ensure maintainability, scalability, and clean separation of concerns.

## Features
- User authentication and authorization
- Order processing and tracking
- Role-based access control
- **Custom API Response Handling** for errors
- **AutoMapper** for object mapping
- **Repository and Service Layer** for data management
- **DTOs** for data transfer
- **Database Seeding** for initial setup

## Technologies Used
- **Backend:** .NET (C#), Entity Framework Core, AutoMapper
- **Database:** SQL Server
- **Architecture:** Onion Architecture

## Installation
### Prerequisites
- .NET SDK
- SQL Server

### Steps
1. Clone the repository:
   ```sh
   git clone https://github.com/Ahmed-Abdrabo/OrderingSystem.git
   cd OrderingSystem
   ```
2. Set up the database:
   - Configure connection string in `appsettings.json`
   - Run migrations:
     ```sh
     dotnet ef database update
     ```
