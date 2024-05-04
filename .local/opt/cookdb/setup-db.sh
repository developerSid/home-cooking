#!/usr/bin/env bash

set -o errexit -o pipefail -o noclobber
PS4='Line ${LINENO}: '

tries=10
while [ $tries -gt 0 ]; do
  if pg_isready; then
    echo "Database is ready"
    break
  else
    echo "Database is not ready, waiting..."
    sleep 1
    tries=$((tries - 1))
  fi
done

if [ $tries -gt 10 ]; then
    echo "Database is not ready, exiting..."
    exit 1
fi

echo "Setting up user and role"
echo Installing required DB extenstions
psql <<EOSQL
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE EXTENSION IF NOT EXISTS pg_stat_statements;
EOSQL

# check if $DATABASE_NAME does not exist
echo "Check if databases have been created"
DOES_NOT_EXISTS=$((psql -t -X --csv) <<EOSQL
SELECT NOT EXISTS(SELECT 1 FROM pg_catalog.pg_database WHERE lower(datname) = '$DATABASE_NAME');
EOSQL
)

# if it does not exist create it
if [ "$DOES_NOT_EXISTS" = "t" ]; then
   echo "Creating databases"
   psql --dbname postgres -c "CREATE USER $DATABASE_USERNAME WITH PASSWORD '$DATABASE_USER_PASSWORD';"
   psql --dbname postgres -c "ALTER USER $DATABASE_USERNAME REPLICATION;"
   psql --dbname postgres -c "CREATE DATABASE ${DATABASE_NAME} OWNER $DATABASE_USERNAME;"
else
    echo "Databases already exists not creating"
fi

psql --dbname "$DATABASE_NAME" -c "GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO $DATABASE_USERNAME;"
psql --dbname "$DATABASE_NAME" -c "GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO $DATABASE_USERNAME;"
psql --dbname "$DATABASE_NAME" -c "ALTER SCHEMA public OWNER TO $DATABASE_USERNAME;"
echo "Finished setting up user, role and databases"