#!/bin/bash
# wait-for-api-dev.sh
set -e

# host="$1"
# service name
host="api"

# port="$2"
port="${API_DOCKER_CONTAINER_PORT}"

until nc -z "$host" "$port"; do
  echo "Waiting for API Development..."
  sleep 2
done

exec "$@"