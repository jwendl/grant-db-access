Push-Location .\src
dotnet publish .\GrantDatabase.CommandLine\GrantDatabase.CommandLine\GrantDatabase.CommandLine.csproj -c Release -r ubuntu.22.04-x64 -p:PublishSingleFile=true --self-contained true
Pop-Location
