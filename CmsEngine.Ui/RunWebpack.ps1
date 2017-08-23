Write-Host "`nRunning " -NoNewline;
Write-Host "webpack --config webpack.config.vendor.js" -NoNewline -ForegroundColor Yellow
Write-Host "...`n";
webpack --config webpack.config.vendor.js

Write-Host "`nRunning " -NoNewline;
Write-Host "webpack" -NoNewline -ForegroundColor Yellow
Write-Host "...`n";
webpack
