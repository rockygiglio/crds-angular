USE MinistryPlatform;
GO

DECLARE @PAGE_ID int = 10;
DECLARE @PAGE_NAME nvarchar(100) = N'Organizations';

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Pages] where [Display_Name] = @PAGE_NAME)
BEGIN

	SET IDENTITY_INSERT [dbo].[dp_Pages] ON

	INSERT INTO [dbo].[dp_Pages] (
		 [Page_ID]
		,[Display_Name]
		,[Singular_Name]
		,[Primary_Key]
		,[Description]
		,[Table_Name]
		,[Default_Field_List]
		,[Selected_Record_Expression]
		,[Display_Copy]
		,[View_Order]
	) VALUES (
		 @PAGE_ID
		,@PAGE_NAME
		,@PAGE_NAME
		,N'Organization_ID'
		,N'Organizations that can participate in GO Cincinnnati or any other volunteer event'
		,N'cr_Organizations'
		,N'Name,Open_Signup,Start_Date,End_Date,Primary_Contact_Table.Display_Name as [Primary_Contact]'
		,N'Name'
		,0
		,100
	)

	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END