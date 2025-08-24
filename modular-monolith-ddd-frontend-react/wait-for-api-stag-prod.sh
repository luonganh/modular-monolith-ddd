#!/bin/bash
# wait-for-api-stag-prod.sh
set -e

# host="$1"
# port="$2"
host="app-api"
port="${API_PORT}"
while ! nc -z "$host" "$port"; do
  echo "Waiting for app-api..."
  sleep 1
done

echo "API ready, starting nginx..."
nginx -g 'daemon off;'