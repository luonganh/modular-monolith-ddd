#!/bin/bash

echo "Starting SQL Server..."
/opt/mssql/bin/sqlservr &

echo "Waiting for SQL Server to start..."
until /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -Q 'SELECT 1' > /dev/null 2>&1; do
  sleep 1
done

echo "Creating database..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -i /docker-entrypoint-initdb.d/CreateDatabase_Linux.sql

echo "Database created."

# Use 'wait' to keep container alive by primary process foreground
wait 
echo "Keep container alive."

## Create another sqlserver instance not foreground of current instance is running background 
# exec /opt/mssql/bin/sqlservr