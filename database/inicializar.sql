CREATE DATABASE ecommerce;
USE ecommerce;
CREATE SCHEMA auth;
CREATE SCHEMA products;
CREATE SCHEMA orders;

CREATE TABLE auth."EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK_EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE products."EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK_EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE orders."EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK_EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);