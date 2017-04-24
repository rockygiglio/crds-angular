USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Processes] WHERE Process_ID = 50)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Processes] ON
	INSERT INTO [dbo].[dp_Processes]
			   ([Process_ID]
			   ,[Process_Name]
			   ,[Process_Manager]
			   ,[Active]
			   ,[Description]
			   ,[Record_Type]
			   ,[Domain_ID]
			   ,[On_Submit]
			   ,[On_Complete]
			   ,[Trigger_Fields]
			   ,[Dependent_Condition])
		 VALUES
			   (50
			   ,'HostApprovedNotification'
			   ,5       -- Church Administrator
			   ,1
			   ,'After Anywhere Gathering Host is approved, send email'
			   ,355     -- Participants
			   ,1
			   ,NULL
			   ,NULL
			   ,'Host_Status_ID'
			   ,'Host_Status_ID=3')
	SET IDENTITY_INSERT [dbo].[dp_Processes] OFF
END

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Process_Steps] WHERE Process_Step_ID = 85)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Process_Steps] ON
	INSERT INTO [dbo].[dp_Process_Steps]
			   ([Process_Step_ID]
			   ,[Step_Name]
			   ,[Instructions]
			   ,[Process_Step_Type_ID]
			   ,[Escalation_Only]
			   ,[Order]
			   ,[Process_ID]
			   ,[Specific_User]
			   ,[Supervisor_User]
			   ,[Lookup_User_Field]
			   ,[Domain_ID]
			   ,[Escalate_to_Step]
			   ,[Task_Deadline]
			   ,[Email_Template]
			   ,[To_Specific_Contact]
			   ,[Email_To_Contact_Field]
			   ,[SQL_Statement])
		 VALUES
			   (85
			   ,'Send Approved Connect Host Notification'
			   ,NULL
			   ,4
			   ,0
			   ,1
			   ,50
			   ,4460578 --crossroads anywhere
			   ,0
			   ,NULL
			   ,1
			   ,NULL
			   ,NULL
			   ,2014
			   ,NULL
			   ,'Participant_ID_Table_Contact_ID_Table.Contact_ID'
			   ,NULL)
	SET IDENTITY_INSERT [dbo].[dp_Process_Steps] OFF
END
GO
