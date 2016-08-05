param (
    [string]$BackupDBServer = "mp-demo-db.centralus.cloudapp.azure.com",
    [string]$RestoreDBServer = "mp-demo-db.centralus.cloudapp.azure.com\TestDBScripts,2433",
    [string]$DBName = "MinistryPlatform", # default to MinistryPlatform
    [string]$ScriptPath = $(throw "-ScriptPath is required."),
    [string]$BackupPath = "F:\Backups\FromProduction",
    [string]$DBUser = $(Get-ChildItem Env:MP_SOURCE_DB_USER).Value, # Default to environment variable
    [string]$DBPassword = $(Get-ChildItem Env:MP_SOURCE_DB_PASSWORD).Value, # Default to environment variable
    [string]$SQLcmd = "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\sqlcmd.exe",
    [boolean]$ForceBackup = $FALSE # Default to use existing backup file   
)

#Use mutex to ensure only 1 process executing against DBServer / DB at a time
$uniqueName = "MPDemoDatabaseRestoreAndRunScripts$DBServer$DBName" 
$singleInstanceMutex = New-Object System.Threading.Mutex($false, $uniqueName)

try
{   
    $singleInstanceMutex.WaitOne()

    .\MPDemoDatabaseBackup.ps1 -DBServer $BackupDBServer -DBName $DBName -BackupPath $BackupPath -DBUser $DBUser -DBPassword $DBPassword -ForceBackup $ForceBackup
 
    if($LASTEXITCODE -eq 0) 
    {
        .\MPTestDatabaseRestore.ps1 -DBServer $RestoreDBServer -DBName $DBName -BackupPath $BackupPath -DBUser $DBUser -DBPassword $DBPassword
    }

    if($LASTEXITCODE -eq 0)
    {
        .\ScriptProcessing.ps1 -DBServer $RestoreDBServer -path $ScriptPath -SQLcmd $SQLcmd -DBUser $DBUser -DBPassword $DBPassword

    }

    exit $LASTEXITCODE
}
finally
{
    $singleInstanceMutex.ReleaseMutex()
    $singleInstanceMutex.Dispose()
}