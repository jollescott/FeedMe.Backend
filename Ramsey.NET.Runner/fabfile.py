from fabric2 import Connection
from invoke import run as local
from invoke import task

@task
def pack(c):
    local('dotnet publish -c release --output ./output')
    local(".\\tools\\7za a -tzip output.zip ./output/*")  

@task
def run(c):
    host = input('Input host name: ')
    user = input('User: ')
    password = input('Password: ')

    if host is None or user is None or password is None:
        print('Incorrect fields.')
        return

    conn = Connection(host=host, user=user,connect_kwargs={"password": password})
    conn.put('output.zip', '/var/ramsey-runner/ramsey.zip')
    conn.run("sudo unzip /var/ramsey-runner/ramsey.zip -d /var/ramsey-runner")
    conn.run("dotnet /var/ramsey-runner/Ramsey.NET.Runner.dll")

