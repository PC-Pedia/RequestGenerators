@ECHO OFF
setlocal
set PROJECT_NAME=RequestGenerators

msbuild src\%PROJECT_NAME%\%PROJECT_NAME%.csproj /p:OutputPath=..\..\Build\%PROJECT_NAME%\Release /p:Configuration=Release
