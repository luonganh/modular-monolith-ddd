#!/bin/bash
set -e

host="${API_HOST:-api}"
port="${API_DOCKER_CONTAINER_PORT}"

until nc -z "$host" "$port"; do
  echo "Waiting for API..."
  sleep "${WAIT_INTERVAL:-2}"
done

echo "API ready, starting app..."
exec "$@"