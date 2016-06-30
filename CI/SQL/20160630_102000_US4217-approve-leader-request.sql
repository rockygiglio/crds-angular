USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Communications] WHERE [Subject]='You''ve been approved to be a Small Group Leader.')
BEGIN

	INSERT INTO [dp_Communications]
		([Author_User_ID],
		[Subject],
		[Body],
		[Domain_ID],
		[Start_Date],
		[Expire_Date],
		[Communication_Status_ID],
		[From_Contact],
		[Reply_to_Contact],
		[_Sent_From_Task],
		[Selection_ID],
		[Template],
		[Active],
		[To_Contact],
		[Time_Zone],
		[Locale])
	VALUES(5,
		'You''ve been approved to be a Small Group Leader.',
		'Thanks [Nickname]!<div style="margin: 0px; padding: 0px; font-family: Verdana; font-size: 12px; background-color: rgb(255, 255, 255);">You have been approved to be a Small Group Leader at Crossroads!</div>',
		1,
		CAST('20160630 00:00:00.000' as DATETIME),
		NULL,
		NULL,
		1519180,
		1519180,
		NULL,
		NULL,
		1,
		1,
		NULL,
		NULL,
		NULL)
END
GO