
# Start SQL Server if it is not already running
$running = $(docker ps -f name=sqlserver --format json) | ConvertFrom-Json
if (-not $running -or $running.Length -eq 0) {
    Write-Host "Starting SQL Server..."
    docker run `
        -e ACCEPT_EULA=1 `
        -e MSSQL_SA_PASSWORD=MyPass@word `
        -e MSSQL_PID=Developer `
        -e MSSQL_USER=SA `
        -p 1433:1433 `
        -d `
        --name=sqlserver `
        mcr.microsoft.com/azure-sql-edge
} else {
    Write-Host "SQL Server is already running."
}

# Start the database worker
Write-Host "Starting the database worker..."
$dbworker = Start-Process -NoNewWindow -PassThru "dotnet" "run --project ./Exciting.DatabaseWorker/Exciting.DatabaseWorker.csproj"
Write-Host "Database worker started."

# Start the web api
Write-Host "Starting the Team API..."
$teamapi = Start-Process -NoNewWindow -PassThru "dotnet" "run --project ./Exciting.TeamApi/Exciting.TeamApi.csproj --launch-profile http"
Write-Host "Team API started."

# Start the website
Write-Host "Starting the website..."
$website = Start-Process -NoNewWindow -PassThru "dotnet" "run --project ./Exciting.Website/Exciting.Website.csproj --launch-profile http"
Write-Host "Website started."

Write-Host "Database Worker: $dbworker"
Write-Host "Team API: $teamapi"
Write-Host "Website: $website"

$procs = $dbworker, $teamapi, $website
Read-Host -Prompt "Press any key to stop the services..."

Write-Host "Stopping the services..."
$procs | Stop-Process
Get-Process "Exciting.*" | Stop-Process

Write-Host "Services stopped."
