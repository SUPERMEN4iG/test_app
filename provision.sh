echo 'INSTALL SCRIPT RUN'

#defines
MSSQL_SA_PASSWORD=rTeRex7CC8CgmCRm
MSSQL_PID=Developer
MSSQL_TCP_PORT=1433
#defines

curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
sudo add-apt-repository "$(curl https://packages.microsoft.com/config/ubuntu/16.04/mssql-server-2017.list)"
sudo apt-get update
sudo apt-get install -y mssql-server
sudo apt-get install mssql-server-agent

# MSSQL Configuration
sudo ACCEPT_EULA='Y' MSSQL_PID=$MSSQL_PID MSSQL_SA_PASSWORD=$MSSQL_SA_PASSWORD MSSQL_TCP_PORT=$MSSQL_TCP_PORT  /opt/mssql/bin/mssql-conf setup  
# MSSQL Password: rTeRex7CC8CgmCRm
sleep 3
systemctl status mssql-server

echo PATH="$PATH:/opt/mssql-tools/bin" >> ~/.bash_profile
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc

curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
curl https://packages.microsoft.com/config/ubuntu/16.04/prod.list | sudo tee /etc/apt/sources.list.d/msprod.list
sudo apt-get update
sudo ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev
#sudo apt-get install mssql-server-agent
#echo -e "Y\n" | sudo apt-get install mssql-tools unixodbc-dev
#sudo /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P rTeRex7CC8CgmCRm -Q 'SELECT Name FROM sys.Databases'

echo Restarting SQL Server...
sudo systemctl restart mssql-server

counter=1
errstatus=1
while [ $counter -le 5 ] && [ $errstatus = 1 ]
do
  echo Waiting for SQL Server to start...
  sleep 3
  /opt/mssql-tools/bin/sqlcmd \
    -S localhost \
    -U SA \
    -P $MSSQL_SA_PASSWORD \
    -Q "SELECT @@VERSION" 2>/dev/null
  errstatus=$?
  ((counter++))
done

# Display error if connection failed:
if [ $errstatus = 1 ]
then
  echo Cannot connect to SQL Server, installation aborted
  exit $errstatus
fi

echo 'INSTALL SCRIPT FINISH'