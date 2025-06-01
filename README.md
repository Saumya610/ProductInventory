# ProductInventory

##  Prerequisites
.NET 8.0
Visual Studio 2022 or later
SQL Server

### How to Run Locally

1. Clone the repository
git clone https://github.com/Saumya610/ProductInventory.git

2. Open ProductInventory.sln in Visual Studio.

3. Build the solution (Build Solution or Ctrl+Shift+B).

4. Run the project (Start Debugging or F5).

### Navigate to interactive Swagger UI 
https://localhost:7209/swagger/index.html or  http://localhost:5155

## Database (SQL Server)
This project uses SQL Server as its database. A local SQL Server instance (like SQL Server Express or SQL Server Developer Edition) should be installed and running.

The connection string is already configured in appsettings.json which can be modified to match the local SQL Server settings if needed.

The database will be created automatically when you build and run the project.

## Project Structure
Controllers/ - Contains API controllers
Models/ - Contains data models
DTOs/ - Data Transfer Objects used for requests/responses
Repositories/ - Handles data access logic
Services/ - Business logic
Mappings/ - AutoMapper profiles

## Features
- Create, Read, Update, Delete (CRUD) operations for products
- Stock management (increment/decrement stock)
- API documentation and testing using Swagger UI