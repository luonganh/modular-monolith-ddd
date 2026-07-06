#!/bin/bash
set -e

echo "Waiting for SQL Server..."
until /opt/mssql-tools/bin/sqlcmd -S "${SQLSERVER_SERVICE_NAME},${SQLSERVER_DOCKER_CONTAINER_PORT}" -U sa -P "$SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1; do
  sleep 2
done

echo "Creating database..."
/opt/mssql-tools/bin/sqlcmd \
  -S "${SQLSERVER_SERVICE_NAME},${SQLSERVER_DOCKER_CONTAINER_PORT}" \
  -U sa \
  -P "$SA_PASSWORD" \
  -i /CreateDatabase_Linux.sql

echo "Database initialized."