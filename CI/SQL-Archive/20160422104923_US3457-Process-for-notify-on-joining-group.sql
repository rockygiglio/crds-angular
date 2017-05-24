USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Processes] WHERE Process_ID = 36)
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
			   (36
			   ,'JoinGroupNotification'
			   ,1529664
			   ,1
			   ,NULL
			   ,316
			   ,1
			   ,NULL
			   ,NULL
			   ,'Group_Participant_ID'
			   ,'EXISTS(select 1 from Groups G where Group_Type_ID = 22 and (G.Group_ID = Group_Participants.Group_ID)')
	SET IDENTITY_INSERT [dbo].[dp_Processes] OFF
END

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Process_Steps] WHERE Process_Step_ID = 74)
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
			   (74
			   ,'Send Notification'
			   ,NULL
			   ,4
			   ,0
			   ,1
			   ,36
			   ,NULL
			   ,0
			   ,'Participant_ID_Table_Contact_ID_Table_User_Account_Table.User_ID'
			   ,1
			   ,NULL
			   ,NULL
			   ,82
			   ,NULL
			   ,'Group_ID_Table_Primary_Contact_Table.Contact_ID'
			   ,NULL)
	SET IDENTITY_INSERT [dbo].[dp_Process_Steps] ON
END