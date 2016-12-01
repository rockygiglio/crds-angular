# Executes a full database backup of the MinistryPlatform database
# Parameters:
#   -DBServer servername_or_ip   The database server, defaults to MPTest02 (optional)
#   -DBName databaseName         The database to backup (optional, defaults to MinistryPlatform)
#   -BackupPath path_on_server   The directory on the DB server to write the backup file (required)
#   -DBUser user                 The SQLServer user to login to the DBServer (optional, defaults to environment variable MP_SOURCE_DB_USER)
#   -DBPassword password         The SQLServer password to login to the DBServer (optional, defaults to environment variable MP_SOURCE_DB_PASSWORD)

Param (
  [Parameter(Mandatory=$true)]
  [string]$DBServer,
  [string]$DBName = "MinistryPlatform", # default to MinistryPlatform
  [Parameter(Mandatory=$true)]
  [string]$BackupUrl,
  [string]$DBUser = $(Get-ChildItem Env:MP_SOURCE_DB_USER).Value, # Default to environment variable
  [string]$DBPassword = $(Get-ChildItem Env:MP_SOURCE_DB_PASSWORD).Value, # Default to environment variable
  [Parameter(Mandatory=$true)]
  [string]$StorageCred
)

$connectionString = "Server=$DBServer;uid=$DBUser;pwd=$DBPassword;Database=master;Integrated Security=False;";

$connection = New-Object System.Data.SqlClient.SqlConnection;
$connection.ConnectionString = $connectionString;
$connection.Open();

$backupDateStamp = Get-Date -format 'yyyyMMdd';

$BackupUrl = "$BackupUrl/$DBName-Backup-$backupDateStamp.trn";
$backupDescription="$DBName - Full Database Backup $backupDateStamp"

$backupSql = @"
USE [master];
BACKUP DATABASE [$DBName]
TO URL = N'$backupUrl' 
WITH CREDENTIAL = N'$StorageCred', COPY_ONLY, NOFORMAT, INIT, NAME = N'$backupDescription', SKIP, NOREWIND, NOUNLOAD, STATS = 10, COMPRESSION;
"@;

$command = $connection.CreateCommand();
$command.CommandText = "$backupSql";
$command.CommandTimeout = 600000;

$exitCode = 0;
$exitMessage = "Success";

echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Beginning backup to file $BackupUrl"
try {
  $command.ExecuteNonQuery();
} catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Backup failed: " + $_.Exception.Message;
}
echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Finished backup to file $BackupUrl"

echo "Status: $exitMessage"
exit $exitCode
