CREATE DATABASE DemoData;
GO
USE DemoData;
GO
CREATE TABLE Products
(
    ID    int            NOT NULL IDENTITY (1,1) PRIMARY KEY,
    Name  nvarchar(255)  NULL,
    Price numeric(19, 2) NULL,
    Stock int            NOT NULL
);
GO
INSERT INTO Products (Name, Price, Stock)
VALUES ('Arm64 SoC', 30.00, 600);
GO
INSERT INTO Products (Name, Price, Stock)
VALUES ('IoT breakout board', 8.00, 400);
GO
INSERT INTO Products (Name, Price, Stock)
VALUES ('DAC extension board', 15.50, 750);
GO