This project implements a .NET Core Web API for managing car models and calculating salesman commissions. The database structure is defined using SQL queries, which are provided below.

Features
CRUD operations for car models.
Dynamic commission calculation based on sales data.
SQL scripts for database and table creation.
Database Setup
1. Database Creation
Run the following query to create the database:

sql
Copy code
CREATE DATABASE CarManagementDB;
USE CarManagementDB;
2. Table Creation Queries
SalesData Table
Stores the monthly sales data for each salesman.

sql
Copy code
CREATE TABLE SalesData (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Salesman NVARCHAR(50) NOT NULL,
    Class NVARCHAR(10) NOT NULL,
    Brand NVARCHAR(50) NOT NULL,
    NumberOfCarsSold INT NOT NULL,
    ModelPrice DECIMAL(18, 2) NOT NULL
);
PreviousYearSales Table
Stores the total sales amount for each salesman from the previous year.

sql
Copy code
CREATE TABLE PreviousYearSales (
    Salesman NVARCHAR(50) PRIMARY KEY,
    TotalSalesAmount DECIMAL(18, 2) NOT NULL
);
SalesmanCommission Table
Stores the calculated commission for each salesman.

sql
Copy code
CREATE TABLE SalesmanCommission (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Salesman NVARCHAR(50) NOT NULL,
    Brand NVARCHAR(50) NOT NULL,
    TotalCommission DECIMAL(18, 2) NOT NULL
);
3. Insert Sample Data
Insert Data into SalesData Table
sql
Copy code
INSERT INTO SalesData (Salesman, Class, Brand, NumberOfCarsSold, ModelPrice)
VALUES
('Salesman 1', 'A', 'Audi', 1, 26000),
('Salesman 1', 'A', 'Jaguar', 3, 36000),
('Salesman 1', 'A', 'Renault', 6, 21000),
('Salesman 2', 'A', 'Jaguar', 5, 36000),
('Salesman 3', 'A', 'Audi', 4, 26000),
('Salesman 3', 'A', 'Renault', 6, 21000);
Insert Data into PreviousYearSales Table
sql
Copy code
INSERT INTO PreviousYearSales (Salesman, TotalSalesAmount)
VALUES
('Salesman 1', 490000),
('Salesman 2', 1000000),
('Salesman 3', 650000);
API Endpoints
GET /api/reports/commission: Calculates and stores commission data based on sales and previous year data.
GET /api/reports/commission/saved: Retrieves previously saved commission data.
How to Use
Clone the repository or download the code.
Configure the database connection string in appsettings.json.
Run the SQL scripts provided above to set up the database and tables.
Build and run the project using Visual Studio or the dotnet run command.
