Push-Location .\src\GrantDatabase.CommandLine
dotnet publish .\GrantDatabase.CommandLine.csproj -c Release -r ubuntu.22.04-x64 -p:PublishSingleFile=true --self-contained true
Pop-Location
