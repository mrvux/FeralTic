Copy-Item ..\Core\bin\x86\Release\FeralTic.dll .\build\net40\lib\x86\FeralTic.dll
Copy-Item ..\Core\bin\x64\Release\FeralTic.dll .\build\net40\lib\x64\FeralTic.dll
nuget pack -NoPackageAnalysis