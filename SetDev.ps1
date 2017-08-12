# Set envitonment to 'Development'
$Env:ASPNETCORE_ENVIRONMENT = "Development"

Write-Host "`nEnvironment set to " -NoNewline;
Write-Host "Development`n" -ForegroundColor Green

# Set url to 'cmsengine.dev'
$env:ASPNETCORE_URLS="http://cmsengine.dev:5000"

Write-Host "Url set to " -NoNewline;
Write-Host "cmsengine.dev`n" -ForegroundColor Green
