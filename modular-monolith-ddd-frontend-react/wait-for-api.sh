#!/bin/bash
# wait-for-api.sh
set -e

# host="$1"
# port="$2"
host="app-api"
port="${API_PORT}"

until nc -z "$host" "$port"; do
  echo "Waiting for app-api..."
  sleep 2
done

exec "$@"