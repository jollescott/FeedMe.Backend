from fabric2 import Connection
from invoke import run as local
from invoke import task
import yaml

mate_config = yaml.safe_load(open("./ramsey.yml"))

print(mate_config)

conn = Connection(host='104.248.245.112', user='root', 
        connect_kwargs={'key_filename': mate_config['fabric']['ssh_key_path'], 'password': mate_config['fabric']['ssh_key_phrase']})

@task
def pack(c):
    local('dotnet publish -c Release -r win-x64 --output ./output')

@task
def deploy(c):
    conn.put('output/', '/var/www/ramsey')

