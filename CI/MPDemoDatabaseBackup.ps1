# Executes a full database backup of the MinistryPlatform database
# Parameters:
#   -DBServer servername_or_ip   The database server, defaults to MPTest02 (optional)
#   -DBName databaseName         The database to backup (optional, defaults to MinistryPlatform)
#   -BackupPath path_on_server   The directory on the DB server to write the backup file (required)
#   -DBUser user                 The SQLServer user to login to the DBServer (optional, defaults to environment variable MP_SOURCE_DB_USER)
#   -DBPassword password         The SQLServer password to login to the DBServer (optional, defaults to environment variable MP_SOURCE_DB_PASSWORD)
#   -ForceBackup force           Force the backup to execute reguardless of existing backup (optional, defaults to false)

Param (
  [string]$DBServer = "mp-demo-db.centralus.cloudapp.azure.com", # default to external IP for MPTest02
  [string]$DBName = "MinistryPlatform", # default to MinistryPlatform
  [string]$BackupPath = "F:\Backups",
  [string]$DBUser = $(Get-ChildItem Env:MP_SOURCE_DB_USER).Value, # Default to environment variable
  [string]$DBPassword = $(Get-ChildItem Env:MP_SOURCE_DB_PASSWORD).Value, # Default to environment variable
  [boolean]$ForceBackup = $FALSE # Default to use existing backup file
)

$backupDateStamp = Get-Date -format 'yyyyMMdd';
$backupFileName="$DBName-Backup-$backupDateStamp.trn";
$backupFileNameFull="$BackupPath\$backupFileName";
$backupDescription="$DBName - Full Database Backup $backupDateStamp"

$connectionString = "Server=$DBServer;uid=$DBUser;pwd=$DBPassword;Database=master;Integrated Security=False;";

$connection = New-Object System.Data.SqlClient.SqlConnection;
$connection.ConnectionString = $connectionString;
$connection.Open();

$backupAlreadyExists = @"
WITH LastBackup AS
(
    SELECT
        database_name,
        backup_finish_date,
		physical_device_name,
        rownum = 
            ROW_NUMBER() OVER
            (
                PARTITION BY database_name
                ORDER BY backup_finish_date DESC
            )
    FROM msdb.dbo.backupset bs
		INNER JOIN msdb..BackupMediaFamily bmf ON [bs].[media_set_id] = [bmf].[media_set_id]
	WHERE 
		-- Database Backup
		type = 'D'
)
SELECT *
FROM [LastBackup] b
WHERE 
	b.[RowNum] = 1 AND
	b.Database_Name = '$DBName' AND 
	b.Physical_Device_Name like '$BackupPath%$backupFileName'
"@;

if ($ForceBackup -eq $FALSE)
{
    $lastRestoreCommand = $connection.CreateCommand();
    $lastRestoreCommand.CommandText = "$backupAlreadyExists";
    $lastRestoreCommand.CommandTimeout = 10000;    

    $reader = $lastRestoreCommand.ExecuteReader();
    try
    {
        if($reader.HasRows)
        {
            # We have resuls so skip backup
            echo "Status: Skipping backup since backup file already exists";
            exit 0;
        }
    }
    finally
    {
        $reader.Close();
        $reader.Dispose();
    }
}

$backupSql = @"
USE [master];
BACKUP DATABASE [$DBName]
TO DISK = N'$backupFileNameFull'
WITH COPY_ONLY, NOFORMAT, INIT, NAME = N'$backupDescription', SKIP, NOREWIND, NOUNLOAD, STATS = 10;
"@

$command = $connection.CreateCommand();
$command.CommandText = "$backupSql";
$command.CommandTimeout = 600000;

$exitCode = 0;
$exitMessage = "Success";

echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Beginning backup to file $backupFileNameFull on server $DBServer"
try {
  $command.ExecuteNonQuery();
} catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Backup failed: " + $_.Exception.Message;
}
echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Finished backup to file $backupFileNameFull on server $DBServer"

echo "Status: $exitMessage"
exit $exitCode
