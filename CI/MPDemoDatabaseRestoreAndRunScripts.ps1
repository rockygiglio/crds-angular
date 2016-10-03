param (
    [string]$BackupDBServer = "mp-demo-db.centralus.cloudapp.azure.com",
    [string]$RestoreDBServer = "mp-demo-db.centralus.cloudapp.azure.com\TestDBScripts,2433",
    [string]$DBName = "MinistryPlatform", # default to MinistryPlatform
    [string]$ScriptPath = $(throw "-ScriptPath is required."),
    [string]$BackupPath = "F:\Backups\FromProduction",
    [string]$DBUser = $(Get-ChildItem Env:MP_SOURCE_DB_USER).Value, # Default to environment variable
    [string]$DBPassword = $(Get-ChildItem Env:MP_SOURCE_DB_PASSWORD).Value, # Default to environment variable
    [string]$SQLcmd = "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\sqlcmd.exe",
    [boolean]$ForceBackup = $FALSE # Default to use existing backup file,
	[string]$changeLogFile = "changelog.txt"
)

#This line could be it's own build step. Or we can potentially just reference %system.teamcity.build.changedFiles.file% in the Get-Content call. 
copy "%system.teamcity.build.changedFiles.file%" changelog.txt

$SQLChanges = @(Get-Content $changeLogFile | Where-Object {$_.StartsWith("CI/SQL")}).Count

if($SQLChanges -gt 0)
{
    Write-Host SQL Changes:
    Write-Host (Get-Content $changeLogFile | Where-Object {$_.StartsWith("CI/SQL")})
	
	#Use mutex to ensure only 1 process executing against DBServer / DB at a time
	$uniqueName = "MPDemoDatabaseRestoreAndRunScripts$DBServer$DBName" 
	$singleInstanceMutex = New-Object System.Threading.Mutex($false, $uniqueName)

	try
	{   
		$singleInstanceMutex.WaitOne()

		.\CI\MPDemoDatabaseBackup.ps1 -DBServer $BackupDBServer -DBName $DBName -BackupPath $BackupPath -DBUser $DBUser -DBPassword $DBPassword -ForceBackup $ForceBackup
 
    if($LASTEXITCODE -eq 0) 
    {
        .\CI\MPTestDatabaseRestore.ps1 -DBServer $RestoreDBServer -DBName $DBName -BackupPath $BackupPath -DBUser $DBUser -DBPassword $DBPassword
    }

    if($LASTEXITCODE -eq 0)
    {
        .\CI\ScriptProcessing.ps1 -DBServer $RestoreDBServer -path $ScriptPath -SQLcmd $SQLcmd -DBUser $DBUser -DBPassword $DBPassword

    }

    exit $LASTEXITCODE
	}
	finally
	{
		$singleInstanceMutex.ReleaseMutex()
		$singleInstanceMutex.Dispose()
	}
		
	}
else 
{
    Write-Host No database changes found.
}

