USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON
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
           ,[Filter_Clause]
           ,[Start_Date_Field]
           ,[End_Date_Field]
           ,[Contact_ID_Field]
           ,[Default_View]
           ,[Pick_List_View]
           ,[Image_Name]
           ,[Direct_Delete_Only]
           ,[System_Name]
           ,[Date_Pivot_Field]
           ,[Custom_Form_Name]
           ,[Display_Copy])
     VALUES
           (558,
		   'Override Email Prevention',
		   'Override Email Prevention',
		   'Any contacts in this list will receive email sent to them from within the integration and demo environments. Contacts NOT on this list will not receive emails in integration or dev, regardless of if they are subscribed or unsubscribed. NO effect in prod.',
		   0,
		   'cr_Override_Email_Prevention',
		   'Override_Email_Prevention_ID',
		   NULL,
		   'cr_Override_Email_Prevention.[Contact_ID]',
		   'Override_Email_Prevention_ID',
		   NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO
