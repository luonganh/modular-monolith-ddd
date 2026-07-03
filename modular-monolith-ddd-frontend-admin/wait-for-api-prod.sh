#!/bin/bash
# wait-for-api-prod.sh
set -e

# host="$1"
# service name
host="api"

# port="$2"
port="${API_DOCKER_CONTAINER_PORT}"
while ! nc -z "$host" "$port"; do
  echo "Waiting for api prod..."
  sleep 1
done

echo "API ready, starting nginx..."
nginx -g 'daemon off;'