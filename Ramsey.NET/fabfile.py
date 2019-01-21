from fabric2 import Connection
from invoke import run as local
from invoke import task
import yaml

mate_config = yaml.safe_load(open("./ramsey.yml"))

conn = Connection(host='104.248.245.112', user='root', 
        connect_kwargs={'key_filename': mate_config['fabric']['ssh_key_path'], 'password': mate_config['fabric']['ssh_key_phrase']})

@task
def pack(c):
    local('dotnet restore')
    local('dotnet publish -c release -r ubuntu.16.10-x64 --output ./output')
    local(".\\tools\\7za a -tzip output.zip ./output/*")  

@task
def deploy(c):
    conn.run("sudo rm -rf /var/www/ramsey/*")
    conn.put('output.zip', '/var/www/ramsey/ramsey.zip')
    conn.run("sudo unzip /var/www/ramsey/ramsey.zip -d /var/www/ramsey")
    conn.run("export ASPNETCORE_ENVIRONMENT")
    conn.run("chmod +x /var/www/ramsey/Ramsey.NET")
    conn.run("sudo systemctl restart ramsey")

