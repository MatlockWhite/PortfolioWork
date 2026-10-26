-- PortfolioWork - UserService
-- MySQL Docker initialization script
-- Creates the database schema for a fresh installation.

CREATE DATABASE IF NOT EXISTS `UserService`;

USE `UserService`;

-- Create Role table first because User references Role.
CREATE TABLE IF NOT EXISTS `role` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleKey` varchar(255) NOT NULL,
    `DisplayName` varchar(255) DEFAULT NULL,
    `Description` varchar(255) DEFAULT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `RoleKey_UNIQUE` (`RoleKey`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;

-- Create User table.
CREATE TABLE IF NOT EXISTS `user` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Username` varchar(255) NOT NULL,
    `PasswordHash` varchar(255) NOT NULL,
    `FirstName` varchar(255) NOT NULL,
    `LastName` varchar(255) NOT NULL,
    `Team` varchar(255) DEFAULT NULL,
    `Email` varchar(255) NOT NULL,
    `PhoneNumber` varchar(25) DEFAULT NULL,
    `RoleId` int DEFAULT NULL,
    `IsActive` tinyint NOT NULL DEFAULT '1',
    `LastActive` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Email_UNIQUE` (`Email`),
    UNIQUE KEY `Username_UNIQUE` (`Username`),
    KEY `Role=_Id_FK_idx` (`RoleId`),
    CONSTRAINT `Role_Id_FK`
        FOREIGN KEY (`RoleId`) REFERENCES `role` (`Id`)
) ENGINE=InnoDB
  DEFAULT CHARSET=utf8mb4
  COLLATE=utf8mb4_0900_ai_ci;