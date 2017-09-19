SET VERSION=0.2.0

cd src\
dotnet restore Tp.I18n.Core.sln
dotnet build -c Release Tp.I18n.Core.sln
dotnet pack --no-build -c Release -o packages Tp.I18n.Core.sln
cd ..


nuget\nuget.exe push src\Tp.I18n\packages\Tp.I18n.Core.%VERSION%.nupkg -Source "https://www.nuget.org/api/v2/package"
pause
