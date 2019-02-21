from fabric2 import Connection
from invoke import run as local
from invoke import task

@task
def pack(c):
    local('dotnet publish -c release -r linux-arm --output ./output')
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
    conn.run("sudo rm -rf /home/pi/ramsey-runner/*")
    conn.put('output.zip', '/home/pi/ramsey-runner/ramsey.zip')
    conn.run("sudo unzip /home/pi/ramsey-runner/ramsey.zip -d /home/pi/ramsey-runner")
    conn.run("sudo chmod +x /home/pi/ramsey-runner/Ramsey.NET.Runner")
    conn.run("/home/pi/ramsey-runner/Ramsey.NET.Runner")


