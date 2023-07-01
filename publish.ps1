Push-Location .\src\GrantDatabase.CommandLine
dotnet publish .\GrantDatabase.CommandLine.csproj -c Release -r linux-amd64 -p:PublishSingleFile=true --self-contained true
Pop-Location
