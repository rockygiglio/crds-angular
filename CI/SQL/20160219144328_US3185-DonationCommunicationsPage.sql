USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Pages] WHERE [Display_Name] = 'Donation Communications')
BEGIN
INSERT INTO [dbo].[dp_Pages]
           ([Page_ID]
		   ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Display_Search]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Display_Copy])
     VALUES
           (540
		   ,'Donation Communications'
           ,'Donation Communication'
           ,'Communication Messages for Donations'
           ,100
           ,'cr_Donation_Communications'
           ,'Donation_Communications_ID'
           ,1
           ,'Donation_Communications_ID, Donation_ID, Communication_ID'
           ,'cr_Donation_Communications.Donation_Communications_ID'
           ,0)
END

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Page_Section_Pages] WHERE Page_ID=540 AND Page_Section_ID=9)
BEGIN
	INSERT INTO [dbo].[dp_Page_Section_Pages]
		([Page_ID],
		[Page_Section_ID],
		[User_ID]) 
	VALUES
		(540,
		9,
		NULL)
END
