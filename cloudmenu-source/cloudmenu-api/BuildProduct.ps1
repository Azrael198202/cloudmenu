if (Test-Path "D:\D-GakuWkSp\projects\103_cloudmenu-laoshanghai\cloudmenu-source\cloudmenu-api\bin\Release\net5.0\publish") {
    Remove-Item "D:\D-GakuWkSp\projects\103_cloudmenu-laoshanghai\cloudmenu-source\cloudmenu-api\bin\Release\net5.0\publish" -Recurse
}
if (Test-Path "./appsettings.json") {
    Remove-Item "./appsettings.json"
}
Copy-Item "./appsettings.json.ProductionServer" "./appsettings.json"
dotnet publish --configuration Release
scp -i D:\D-GakuWkSp\sshkey\laoshanghai_test -r D:\D-GakuWkSp\projects\103_cloudmenu-laoshanghai\cloudmenu-source\cloudmenu-api\bin\Release\net5.0\publish\* root@192.168.1.17:/home/cloudmenu/api
"sudo systemctl restart cloudmenu_api.service"
"Invoke-WebRequest -Uri 'http://cloudmenu.leadingwin.net/api/lab/getDbVersion'"
ssh laoshanghai_prod 