echo 'INSTALL SCRIPT RUN'

#defines
PSQL_SA_PASSWORD=12345
PSQL_SA_LOGIN=sa
PSQL_TCP_PORT=5432
PSQL_DB_NAME=each
PSQL_VERSION=9.5
PSQL_SUBVERSION=173ubuntu0.1
PSQL_FULLVERSION=$PSQL_VERSION+$PSQL_SUBVERSION
#defines

sudo apt-get update
sudo apt-get install postgresql=$PSQL_FULLVERSION -y

sudo sed -i "s/#listen_address.*/listen_addresses '*'/" /etc/postgresql/$PSQL_VERSION/main/postgresql.conf

cat >/etc/postgresql/$PSQL_VERSION/main/pg_hba.conf << EOF
# Accept all IPv4 connections - FOR DEVELOPMENT ONLY!!!
host    all         all         0.0.0.0/0             md5
EOF

sudo su postgres -c "psql -c \"CREATE ROLE "$PSQL_SA_LOGIN" SUPERUSER LOGIN PASSWORD '"$PSQL_SA_PASSWORD"'\" "
sudo su postgres -c "createdb -E UTF8 -T template0 --locale=en_US.utf8 -O "$PSQL_SA_LOGIN" "$PSQL_DB_NAME""

sudo systemctl restart postgresql 
sleep 3
sudo systemctl status postgresql 

echo 'INSTALL SCRIPT FINISH'