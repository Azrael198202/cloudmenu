# cloudmenu-api

## mysql EntityFrameworkCore
https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql
https://docs.microsoft.com/en-us/ef/core/get-started/overview/install
https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql

## LiteXStorage
https://www.nuget.org/packages/LiteX.Storage.Core/
参照：FileController.java

## .Net Core CLI
dotnet tool install --global dotnet-ef
dotnet ef dbcontext scaffold "Server=192.168.1.18;Database=cloudmenu_lao_shanghai_test;User=developer;Password=root2021;TreatTinyAsBoolean=true;" "Pomelo.EntityFrameworkCore.MySql" --context AppDbContext --context-dir DbContextCloudMenu --force --output-dir DbEntitiesCloudMenu

## logging
https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-5

## publish
dotnet publish --configuration Release
sudo systemctl stop cloudmenu_api.service
D:\D-GakuWkSp\projects\103_cloudmenu-laoshanghai\cloudmenu-source\cloudmenu-api\bin\Release\net5.0\publish\
/home/cloudmenu/api
sudo systemctl restart cloudmenu_api.service
sudo systemctl status cloudmenu_api.service
## publish by powershell
Set-ExecutionPolicy RemoteSigned
.\BuildTest.ps1

### code style
comment block
/*****
*
*
*****/

