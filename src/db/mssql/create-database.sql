CREATE DATABASE DemoData;
GO
USE DemoData;
GO
IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'Products')
    CREATE TABLE Products
    (
        ID    int            NOT NULL IDENTITY (1,1) PRIMARY KEY,
        Name  nvarchar(255)  NULL,
        Price numeric(19, 2) NULL,
        Stock int            NOT NULL
    );
GO
IF NOT EXISTS(SELECT * FROM Products WHERE Name = 'Arm64 SoC')
    INSERT INTO Products (Name, Price, Stock)
    VALUES ('Arm64 SoC', 30.00, 600);
GO
IF NOT EXISTS(SELECT * FROM Products WHERE Name = 'IoT breakout board')
    INSERT INTO Products (Name, Price, Stock)
    VALUES ('IoT breakout board', 8.00, 400);
GO
IF NOT EXISTS(SELECT * FROM Products WHERE Name = 'DAC extension board')
    INSERT INTO Products (Name, Price, Stock)
    VALUES ('DAC extension board', 15.50, 750);
GO