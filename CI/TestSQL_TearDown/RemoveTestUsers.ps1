param (
    [string]$DBServer = "mp-int-db.cloudapp.net",
    [string]$SQLcmd = "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\sqlcmd.exe",
    [string]$DBUser = $(Get-ChildItem Env:MP_SOURCE_DB_USER).Value, # Default to environment variable
    [string]$DBPassword = $(Get-ChildItem Env:MP_SOURCE_DB_PASSWORD).Value # Default to environment variable
 )
 
 $userList = import-csv '..\TestSQL\01.TestUsers\userList.csv'
 $exitCode = 0
 $SQLCommonParams = @("-U", $DBUser, "-P", $DBPassword, "-S", $DBServer, "-b")
 
 foreach($user in $userList)
{
	if(![string]::IsNullOrEmpty($user.email))
	{
		write-host "Removing User" $user.first $user.last "with email" $user.email;
		$output = & $SQLcmd @SQLCommonParams -Q "EXEC [MinistryPlatform].[dbo].[cr_QADeleteData] @Email_Address '$user.email'"
		
		if($LASTEXITCODE -ne 0){
				write-host "User: "$user.email
				write-host "Error: "$output
				$exitCode = $LASTEXITCODE
			}
	}
}
exit $exitCode