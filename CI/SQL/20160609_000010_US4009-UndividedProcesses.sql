USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Processes] WHERE Process_ID = 37)
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
			   (37
			   ,'Undivided:  Request to Facilitate Received'
			   ,4439146
			   ,1
			   ,'This process will send an email to a user who registers to facilitate an Undivided session'
			   ,316
			   ,1
			   ,NULL
			   ,NULL
			   ,'Group_Participant_ID'
			   ,'EXISTS(select 1 from Groups G where Group_Type_ID = 26 and Group_Role_ID = 22 and (G.Group_ID = Group_Participants.Group_ID))')
	SET IDENTITY_INSERT [dbo].[dp_Processes] OFF
END

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Process_Steps] WHERE Process_Step_ID = 75)
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
			   (75
			   ,'Undivided Facilitator Confirmation Email '
			   ,NULL
			   ,4
			   ,0
			   ,1
			   ,37
			   ,1344416
			   ,0
			   ,NULL
			   ,1
			   ,NULL
			   ,NULL
			   ,17553
			   ,NULL
			   ,'Participant_ID_Table_Contact_ID_Table.[Contact_ID]'
			   ,NULL)
	SET IDENTITY_INSERT [dbo].[dp_Process_Steps] OFF
END



IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Processes] WHERE Process_ID = 38)
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
			   (38
			   ,'Undivided:  Request to Participate Received'
			   ,4439146
			   ,1
			   ,'This process will send an email to a user who registers as a participant for an Undivided session'
			   ,316
			   ,1
			   ,NULL
			   ,NULL
			   ,'Group_Participant_ID'
			   ,'EXISTS(select 1 from Groups G where Group_Type_ID = 26 and Group_Role_ID = 16 and (G.Group_ID = Group_Participants.Group_ID))')
	SET IDENTITY_INSERT [dbo].[dp_Processes] OFF
END

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Process_Steps] WHERE Process_Step_ID = 76)
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
			   (76
			   ,'Undivided Participant Confirmation Email'
			   ,NULL
			   ,4
			   ,0
			   ,1
			   ,37
			   ,1344416
			   ,0
			   ,NULL
			   ,1
			   ,NULL
			   ,NULL
			   ,17552
			   ,NULL
			   ,'Participant_ID_Table_Contact_ID_Table.[Contact_ID]'
			   ,NULL)
	SET IDENTITY_INSERT [dbo].[dp_Process_Steps] OFF
END