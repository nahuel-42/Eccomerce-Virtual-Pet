#!/bin/bash
cd /src

# Aplicar migraciones a ProductsDbContext
until dotnet ef database update --context ProductsDbContext --project ecommerce-backend.csproj; do
  echo "Error al aplicar migraciones para ProductsDbContext. Esperando 2 segundos antes de reintentar..."
  sleep 20
done

# Aplicar migraciones a OrdersDbContext
until dotnet ef database update --context OrdersDbContext --project ecommerce-backend.csproj; do
  echo "Error al aplicar migraciones para OrdersDbContext. Esperando 2 segundos antes de reintentar..."
  sleep 2
done

# Aplicar migraciones a UsersDbContext
until dotnet ef database update --context UsersDbContext --project ecommerce-backend.csproj; do
  echo "Error al aplicar migraciones para UsersDbContext. Esperando 2 segundos antes de reintentar..."
  sleep 2
done

echo "Migración completada, iniciando la app..."
cd /app/publish
exec dotnet ecommerce-backend.dll