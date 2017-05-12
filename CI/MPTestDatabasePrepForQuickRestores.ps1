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

echo "Starting database prep for quick restores script at $(Get-Date)"

$backupDateStamp = Get-Date -format 'yyyyMMdd';

$restoreFileName="$DBName-Backup-$backupDateStamp.trn";
$restoreFileNameFull="$BackupPath\$restoreFileName";

$backupFileName="$DBName-Backup-$backupDateStamp-simple.trn";
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
    echo "Checking if existing prepped backup exists at $(Get-Date)"

    $lastRestoreCommand = $connection.CreateCommand();
    $lastRestoreCommand.CommandText = "$backupAlreadyExists";
    $lastRestoreCommand.CommandTimeout = 10000;    

    $reader = $lastRestoreCommand.ExecuteReader();
    try
    {
        if($reader.HasRows)
        {
            # We have resuls so skip backup
            echo "Status: Skipping prepare steps since backup file already exists";
            exit 0;
        }

        echo "Finished checking and prepped database backup does not exists at $(Get-Date)"
    }
    finally
    {
        $reader.Close();
        $reader.Dispose();
    }
}


# Determine the current log and data file locations, so we can relocate from the backup.
# This is needed because the servers are not setup with identical drives and paths.
echo "Determine existing DB file locations at $(Get-Date)"

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

echo "Finished determining existing DB file locations at $(Get-Date)"

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

echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Beginning restore from file $restoreFileName on server $DBServer"
try {
  $command.ExecuteNonQuery();
} catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Restore failed: " + $_.Exception.Message;
}
echo "$(Get-Date -format 'yyyy-MM-dd HH:mm:ss') Finished restore from file $restoreFileName on server $DBServer"

# If the restore failed, exit now
if($exitCode -ne 0) {
  echo "Status: $exitMessage";
  exit $exitCode;
}

# Set DB to simple recovery and shrink to improve restore performance
$simpleAndShrinkSql = @"
USE [master];
ALTER DATABASE [$DBName] SET RECOVERY SIMPLE WITH NO_WAIT
DBCC SHRINKFILE (N'$logFileName' , 0, TRUNCATEONLY)
"@;

$command = $connection.CreateCommand();
$command.CommandText = "$simpleAndShrinkSql";
$command.CommandTimeout = 600000;

$exitCode = 0;
$exitMessage = "Success";

echo "Setting DB to simple recovery and shrinking at $(Get-Date)"
try {
  $command.ExecuteNonQuery();
} catch [System.Exception] {
  $exitCode = 1;
  $exitMessage = "ERROR - Backup failed: " + $_.Exception.Message;
}
echo "Finished setting DB to simple recovery and shrinking at $(Get-Date)"

# Backup database that has been shrunk and set to simple recover mode
$backupSql = @"
USE [master];
BACKUP DATABASE [$DBName]
TO DISK = N'$backupFileNameFull'
WITH COPY_ONLY, NOFORMAT, INIT, NAME = N'$backupDescription', SKIP, NOREWIND, NOUNLOAD, COMPRESSION, STATS = 10;
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