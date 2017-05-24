USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET IDENTITY_INSERT [dbo].[dp_Process_Steps] ON;

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Process_Steps] WHERE Step_Name='Send Small Group Leader Email')
BEGIN

	DECLARE @CommunicationTemplateId INT
	SELECT @CommunicationTemplateId = Communication_ID FROM dp_Communications WHERE Subject='You''ve been approved to be a Small Group Leader.'

	INSERT INTO [dp_Process_Steps]
		([Process_Step_ID],
		[Step_Name],
		[Instructions],
		[Process_Step_Type_ID],
		[Escalation_Only],
		[Order],
		[Process_ID],
		[Specific_User],
		[Supervisor_User],
		[Lookup_User_Field],
		[Domain_ID],
		[Escalate_to_Step],
		[Task_Deadline],
		[Email_Template],
		[To_Specific_Contact],
		[Email_To_Contact_Field],
		[SQL_Statement])
	VALUES
		(79,
		'Send Small Group Leader Email',
		NULL,
		4,
		0,
		1,
		41,
		5,
		0,
		NULL,
		1,
		NULL,
		NULL,
		@CommunicationTemplateId,
		NULL,
		'Participant_ID_Table_Contact_ID_Table.Contact_ID'
		,NULL)

END
GO