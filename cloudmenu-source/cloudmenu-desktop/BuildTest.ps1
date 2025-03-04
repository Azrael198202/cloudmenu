if (Test-Path "./.env.production") {
    Remove-Item "./.env.production"
}
Copy-Item "./.env.production.TestServer" "./.env.production"
yarn electron:build
Invoke-Item ./dist_electron