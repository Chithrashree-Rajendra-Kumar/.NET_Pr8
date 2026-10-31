-- Run this script in SQL Server Management Studio (SSMS)
IF DB_ID('InventoryDB') IS NULL
    CREATE DATABASE InventoryDB;
GO

USE InventoryDB;
GO

IF OBJECT_ID('dbo.Product', 'U') IS NULL
CREATE TABLE dbo.Product
(
    ProductId   INT IDENTITY(1,1) PRIMARY KEY,
    ProductName VARCHAR(100)  NOT NULL,
    Category    VARCHAR(100)  NOT NULL,
    Price       DECIMAL(10,2) NOT NULL,
    Quantity    INT           NOT NULL
);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Product)
INSERT INTO dbo.Product (ProductName, Category, Price, Quantity)
VALUES
('Laptop',   'Electronics', 55000, 10),
('Keyboard', 'Accessories',  1200, 25),
('Mouse',    'Accessories',   700, 40),
('Monitor',  'Electronics', 15000, 15);
GO

SELECT * FROM dbo.Product;
GO
