# Requires Powershell version 4, if this doesn't work you are likely running version 3
# https://www.microsoft.com/en-us/download/confirmation.aspx?id=40855
# Given a directory, parses files and adds meta data to the cr_Build_Scripts
# table.  Also executes script and marks them as executed in the DB.
# Will not execute scripts that are in the DB, but not executed, if this
# becomes a problem we'll have to rework it a bit.

param (
    [string]$DBServer = "mp-int-db.centralus.cloudapp.azure.com",
    [string]$path = $(throw "-path is required."),
    [string]$SQLcmd = "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\sqlcmd.exe",
    [string]$DBUser = $(Get-ChildItem Env:MP_SOURCE_DB_USER).Value, # Default to environment variable
    [string]$DBPassword = $(Get-ChildItem Env:MP_SOURCE_DB_PASSWORD).Value # Default to environment variable
)

$exitCode = 0
$SQLCommonParams = @("-U", $DBUser, "-P", $DBPassword, "-S", $DBServer, "-b")

# Setup ADODB connection
$connectionString = "Server=$DBServer;uid=$DBUser;pwd=$DBPassword;Database=master;Integrated Security=False;";

$connection = New-Object System.Data.SqlClient.SqlConnection;
$connection.ConnectionString = $connectionString;
$connection.Open();    

# Add SQL Message output to the console
$sqlMessageHandler = [System.Data.SqlClient.SqlInfoMessageEventHandler] {param($sender, $event) Write-Host $event.Message };
$connection.add_InfoMessage($sqlMessageHandler);

Write-Output "$(Get-Date) Querying previous scripts"

$query = "SELECT Name, MD5 FROM [MinistryPlatform].[dbo].cr_Scripts";

$command = New-Object System.Data.SQLClient.SQLCommand;
$command.Connection = $connection;
$command.CommandText = $query;    
    
try {
    $reader = $command.ExecuteReader();
    
    $previousScripts = New-Object Collections.Generic.HashSet[string];
    while ($reader.Read()) {
        $key = $reader.GetString(1)
        $previousScripts.Add($key) | Out-Null
    }

    $reader.Close();
    Write-Output "$(Get-Date) Finished querying previous scripts"
}
catch {
    $exceptionMessage = $_.Exception.Message;
    Write-Output "$(Get-Date) Error running SQL at with exception $exceptionMessage"
}
  
Write-Output "Starting to process scripts $(Get-Date)"

Get-ChildItem $path -recurse -filter *.sql | Foreach-Object {
    # TODO: Determine if we can compute MD5 of files with and without CR/LF
    $hashObj = Get-FileHash $_.FullName -Algorithm MD5

    $hash = $hashObj.hash
    $current = $_.Name + $hash;

    $found = $previousScripts.Contains($hash);
    if ($found -eq $false) {
        Write-Output "File $_ was found = $found with hash $hash";

        try {
            $command = New-Object System.Data.SQLClient.SQLCommand;
            $command.Connection = $connection;
            $command.CommandText = "INSERT INTO [MinistryPlatform].[dbo].[cr_Scripts] ([Name] ,[MD5]) VALUES ('$_','$hash')";

            $command.ExecuteNonQuery();
        }
        catch {         
            Write-Output "File: $_"
            Write-Output "Error: $_.Exception.Message"
            $exitCode = 8
        }

    	
        Write-Output "Running script $_"
        $output = & $SQLcmd @SQLCommonParams -I -i $_.FullName
        if ($LASTEXITCODE -ne 0) {
            Write-Output "File: $_"
            Write-Output "Error: $output"
            $exitCode = $LASTEXITCODE
        } 
        else {
            #If the new script executed well mark it as executed
            try {
                $command = New-Object System.Data.SQLClient.SQLCommand;
                $command.Connection = $connection;
                $command.CommandText = "UPDATE [MinistryPlatform].[dbo].[cr_Scripts] set executed=1 where [MD5] = '$hash'";

                $command.ExecuteNonQuery();
            }
            catch {         
                Write-Output "File: $_"
                Write-Output "Error: $_.Exception.Message"
                $exitCode = 8
            }
        }
    }
}
$connection.Close();
Write-Output "Finished $(Get-Date)"
exit $exitCode