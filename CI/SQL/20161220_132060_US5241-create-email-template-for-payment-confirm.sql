use MinistryPlatform
Go

DECLARE @Template_ID int = 2006;
DECLARE @Camp_Contact_ID int = 7676101;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Communications] WHERE [Communication_ID] = @Template_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Communications] ON;
	INSERT INTO [dbo].[dp_Communications] (
		 [Communication_ID]
		,[Author_User_ID]
		,[Subject]
		,[Body]
		,[Domain_ID]
		,[From_Contact]
		,[Reply_to_Contact]
		,[Template]
	) VALUES (
		 @Template_ID
		,5
		,N'[Event_Title] Payment Received'
		,N'Thanks for submitting your payment of $[Payment_Total].<div><br /></div><div>Camps must be paid if full by May 1st. If you need to make additional payments you can do so anytime by going to <a href="https://[Base_Url]/mycamps" target="_self">https://[Base_Url]/mycamps</a> and clicking the Make Payment button. Please note that these payments are <b>NOT</b> tax deductible. </div><div><br /></div><div>If you have any questions, email [Primary_Contact_Email].</div><div><br /></div><div>Thanks,</div><div>[Primary_Contact_Display_Name]</div>'
		,1
		,@Camp_Contact_ID
		,@Camp_Contact_ID
		,1
	);
	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF;
END

-- Update the curent Summer Camps Program with the new template
DECLARE @Program_ID int;
SELECT Top 1 @Program_ID = Program_ID FROM [dbo].[Programs] WHERE Program_Name = 'Summer Camps';

UPDATE [dbo].[Programs] SET [Communication_ID] = @Template_ID WHERE [Program_ID] = @Program_ID;