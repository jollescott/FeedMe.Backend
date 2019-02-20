from fabric2 import Connection
from invoke import run as local
from invoke import task

@task
def pack(c):
    local('dotnet restore')
    local('dotnet publish -c release -r ubuntu.16.10-x64 --output ./output')
    local(".\\tools\\7za a -tzip output.zip ./output/*")  

