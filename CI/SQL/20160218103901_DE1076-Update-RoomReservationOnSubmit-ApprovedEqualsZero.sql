use MinistryPlatform;
go

/** UP **/
IF exists (SELECT 1 FROM [dbo].[dp_Processes] WHERE Process_Name = N'Room Request')
BEGIN
	UPDATE [dbo].[dp_Processes] 
	SET [On_Submit] = N'_Approved=0'
	WHERE [Process_Name] = N'Room Request'
END

/** DOWN **/
--IF exists (SELECT 1 FROM [dbo].[dp_Processes] WHERE Process_Name = N'Room Request')
--BEGIN
--	UPDATE [dbo].[dp_Processes] 
--	SET [On_Submit] = null
--	WHERE [Process_Name] = N'Room Request'
--END