SET Version=0.1.0

nuget\nuget.exe pack src\Tp.I18n\Tp.I18n.csproj -Build -Symbols -Properties Configuration=Release -Version %VERSION%
nuget\nuget.exe push Tp.I18n.%VERSION%.nupkg
