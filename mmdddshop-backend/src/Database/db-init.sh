#!/bin/bash
set -e
: "${SQLSERVER_DATABASE_NAME:?SQLSERVER_DATABASE_NAME is required}"
: "${SQLSERVER_USER:?SQLSERVER_USER is required}"
: "${SQLSERVER_PASSWORD:?SQLSERVER_PASSWORD is required}"

echo "Waiting for SQL Server..."
until /opt/mssql-tools/bin/sqlcmd -S "${SQLSERVER_SERVICE_NAME},${SQLSERVER_DOCKER_CONTAINER_PORT}" -U sa -P "$SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1; do
  sleep 2
done

# sqlcmd reads $(DB_NAME), $(APP_USER), $(APP_PASSWORD) from these environment variables
export DB_NAME="$SQLSERVER_DATABASE_NAME"
export APP_USER="$SQLSERVER_USER"
export APP_PASSWORD="$SQLSERVER_PASSWORD"

echo "Creating database..."
/opt/mssql-tools/bin/sqlcmd \
  -S "${SQLSERVER_SERVICE_NAME},${SQLSERVER_DOCKER_CONTAINER_PORT}" \
  -U sa \
  -P "$SA_PASSWORD" \
  -b \
  -i /CreateDatabase_Linux.sql

echo "Database initialized."