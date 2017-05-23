USE [MinistryPlatform]
GO

-- =============================================
-- Author:      John Cleaver
-- Create date: 2017-05-23
-- Description:	Adds template for invoice detail
-- (Note: ID created via Identity Maintenance proc)
-- =============================================

DECLARE @TemplateID int = 2019
DECLARE @Body VARCHAR(max) = '<div>Hey there!</div><div></div><div><br /></div><div>In order to make a payment, you can do so at any time by going to https://[BASE_URL]/invoices/[INVOICE_ID]. Please note that these payments are <b>NOT tax deductible.</b><br /></div><div><br /></div><div>[EVENT_TITLE] will be from [EVENT_START_DATE] through [EVENT_END_DATE]<div></br>Thanks,</div><div>[DISPLAY_NAME]</div>'
DECLARE @Subject VARCHAR(max) = 'You Have a New Invoice from Crossroads'

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Communications] WHERE Communication_ID = @TemplateID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Communications] ON
	INSERT INTO [dp_Communications]
	(
		 [Communication_ID]
		,[Author_User_ID]
		,[Subject]
		,[Body]
		,[Domain_ID]
		,[Start_Date]
		,[From_Contact]
		,[Reply_to_Contact]
		,[Template]
		,[Active]
	)
	VALUES
	(
		 @TemplateID
		,5
	    ,@Subject
		,@Body
		,1
		,GetDate()
		,7656635 -- finance@crossroads.net
		,7656635 -- finance@crossroads.net
		,1
		,1
	)

	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END
