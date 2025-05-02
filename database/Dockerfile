# Use an official PostgreSQL image as the base image
FROM postgres:15

# Set environment variables for PostgreSQL
ENV POSTGRES_DB=ecommerce
ENV POSTGRES_USER=ecommerce
ENV POSTGRES_PASSWORD=1111

# Copy the initialization script to the Docker image
COPY inicializar.sql /docker-entrypoint-initdb.d/

# Expose the default PostgreSQL port
EXPOSE 5432
