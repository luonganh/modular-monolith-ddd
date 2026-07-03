#!/bin/bash
# wait-for-api-staging.sh
set -e

# host="$1"
# service name
host="api"

# port="$2"
port="${API_DOCKER_CONTAINER_PORT}"

until nc -z "$host" "$port"; do
  echo "Waiting for api staging..."
  sleep 2
done

exec "$@"