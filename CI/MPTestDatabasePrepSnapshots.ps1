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

Write-Output "Starting database prep for quick restores script at $(Get-Date)"

$backupDateStamp = Get-Date -format 'yyyyMMdd';

$restoreFileName="$DBName-Backup-$backupDateStamp.trn";
$restoreFileNameFull="$BackupPath\$restoreFileName";

$connectionString = "Server=$DBServer;uid=$DBUser;pwd=$DBPassword;Database=master;Integrated Security=False;";

$connection = New-Object System.Data.SqlClient.SqlConnection;
$connection.ConnectionString = $connectionString;
$connection.Open();

# Add SQL Message output to the console
$sqlMessageHandler = [System.Data.SqlClient.SqlInfoMessageEventHandler] {param($sender, $event) Write-Host $event.Message };
$connection.add_InfoMessage($sqlMessageHandler);

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
    Write-Output "Checking if existing prepped backup exists at $(Get-Date)"

    $lastRestoreCommand = $connection.CreateCommand();
    $lastRestoreCommand.CommandText = "$backupAlreadyExists";
    $lastRestoreCommand.CommandTimeout = 10000;    

    $reader = $lastRestoreCommand.ExecuteReader();
    try
    {
        if($reader.HasRows)
        {
            # We have resuls so skip backup
            Write-Output "Status: Skipping prepare steps since backup file already exists";
            exit 0;
        }

        Write-Output "Finished checking and prepped database backup does not exists at $(Get-Date)"
    }
    finally
    {
        $reader.Close();
        $reader.Dispose();
    }
}


# Determine the current log and data file locations, so we can relocate from the backup.
# This is needed because the servers are not setup with identical drives and paths.
Write-Output "Determine existing DB file locations at $(Get-Date)"

$sql = @"
SELECT type, name, physical_name
FROM sys.master_files
WHERE [database_id] = DB_ID('$DBName')
ORDER BY type, name;
"@;

$command = $connection.CreateCommand();
$command.CommandText = "$sql";

$reader = $command.ExecuteReader();

$table = New-Object System.Data.DataTable;
$table.Load($reader);

$dataFile = $table | Where-Object {$_.type -eq 0};
$logFile = $table | Where-Object {$_.type -eq 1};

$dataFileName = $dataFile.name;
$dataFilePhysicalName = $dataFile.physical_name;

$logFileName = $logFile.name;
$logFilePhysicalName = $logFile.physical_name;

Write-Output "Finished determining existing DB file locations at $(Get-Date)"

#Determine default location for DB
Write-Output "Determine Default file locations at $(Get-Date)"

$sql = @"
SELECT SERVERPROPERTY('INSTANCEDEFAULTDATAPATH')
"@;

$command = $connection.CreateCommand();
$command.CommandText = "$sql";

$reader = $command.ExecuteReader();

$defaultFileLocation = $reader.GetString(0);

Write-Output "Finished determining Default file locations at $(Get-Date)"

# Delete existing snapshots if they existing
$removeSnapshotSql = @"
DECLARE @database VARCHAR(100)
DECLARE @sql NVARCHAR(4000)
DECLARE cur_db CURSOR FOR
	SELECT name
	FROM sys.databases
	WHERE
		source_database_id IN (SELECT database_id FROM sys.databases WHERE name = '$DBName')
	ORDER BY create_date DESC
	FOR READ ONLY

OPEN cur_db
FETCH NEXT FROM cur_db INTO @database
WHILE (@@FETCH_STATUS = 0 )
BEGIN

	SET @sql = 'ALTER DATABASE ['+@database+'] SET OFFLINE WITH ROLLBACK IMMEDIATE;'
	EXEC sp_executesql @sql
	SET @sql = 'DROP DATABASE ['+@database+'];'
	EXEC sp_executesql @sql
	FETCH NEXT FROM cur_db INTO @database
END
CLOSE cur_db
DEALLOCATE cur_db
"@;

$command = $connection.CreateCommand();
$command.CommandText = "$removeSnapshotSql";
$command.CommandTimeout = 600000;

$exitCode = 0;
$exitMessage = "Success";

Write-Output "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Beginning delete of snapshots of $DBName database"
try {
  $command.ExecuteNonQuery();
} catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Restore failed: " + $_.Exception.Message;
}
Write-Output "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Finished deleting snapshots of $DBName database"

# Restore the database - need to take it offline, restore, then bring back online
$restoreSql = @"
USE [master];

ALTER DATABASE [$DBName] SET OFFLINE WITH ROLLBACK IMMEDIATE;

RESTORE DATABASE [$DBName]
FROM DISK = N'$restoreFileNameFull'
WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 5,
MOVE N'$logFileName' TO N'$logFilePhysicalName',
MOVE N'$dataFileName' TO N'$dataFilePhysicalName';

ALTER DATABASE [$DBName] SET ONLINE;
"@;

$command = $connection.CreateCommand();
$command.CommandText = "$restoreSql";
$command.CommandTimeout = 600000;

$exitCode = 0;
$exitMessage = "Success";

Write-Output "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Beginning restore from file $restoreFileName on server $DBServer"
try {
  $command.ExecuteNonQuery();
} catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Restore failed: " + $_.Exception.Message;
}
Write-Output "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Finished restore from file $restoreFileName on server $DBServer"

# If the restore failed, exit now
if($exitCode -ne 0) {
  Write-Output "Status: $exitMessage";
  exit $exitCode;
}

# Create a snapshot of the database for quick restores
$snapshotFilename = $defaultFileLocation + $DBName + "_snapshot.ss"; 
$snapshotDBName = $DBName + "_Snapshot";

$createSnapshotSql = @"
USE [$DBName];

CREATE DATABASE ON $snapshotDBName
( 
    NAME = $DBName, 
    FILENAME = '$snapshotFilename'
)
AS SNAPSHOT OF $DBName;
"@;

$command = $connection.CreateCommand();
$command.CommandText = "$createSnapshotSql";
$command.CommandTimeout = 600000;

$exitCode = 0;
$exitMessage = "Success";

Write-Output "Creating Snapshot DB at $(Get-Date)"
try {
  $command.ExecuteNonQuery();
} catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Creating snapshot failed: " + $_.Exception.Message;
}
Write-Output "Finished creating Snapshot DB at $(Get-Date)"

Write-Output "Status: $exitMessage"
exit $exitCode