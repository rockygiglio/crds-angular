USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET IDENTITY_INSERT [dbo].[dp_Processes] ON;

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Processes] WHERE Process_Name='Send Small Group Leader Approval Email')
BEGIN
	INSERT INTO [dp_Processes]
		([Process_ID],
		[Process_Name],
		[Process_Manager],
		[Active],
		[Description],
		[Record_Type],
		[Domain_ID],
		[On_Submit],
		[On_Complete],
		[Trigger_Fields],
		[Dependent_Condition])
	VALUES(41,
		'Send Small Group Leader Approval Email',
		5,
		1,
		'This process sends an email to a Contact when they are approved as a Group Leader.',
		355,
		1,
		NULL,
		NULL,
		'Approved_Small_Group_Leader',
		'Approved_Small_Group_Leader=1')
END

SET IDENTITY_INSERT [dbo].[dp_Processes] OFF;

GO