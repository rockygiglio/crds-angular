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
           ,[Display_Copy])
     VALUES
           (35,
		   'Childcare Request Statuses'
           ,'Childcare Request Status'
           ,'Display childcare request statuses used by childcare site owners'
           ,1
           ,'cr_Childcare_Request_Statuses'
           ,'Childcare_Request_Status_ID'
           , null
           ,'Childcare_Request_Status_ID,Request_Status'
           ,'Childcare_Request_Status_ID'
           ,1);

	 SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO

DECLARE @CHILDCARE_REQUEST_STATUS_PAGE int;
DECLARE @PAGE_SECTION int;

SELECT @CHILDCARE_REQUEST_STATUS_PAGE = [Page_ID] FROM  [dbo].[dp_Pages] p
	WHERE p.Display_Name = N'Childcare Request Statuses'

SELECT @PAGE_SECTION = [Page_Section_ID] FROM
	[dbo].[dp_Page_Sections]
	WHERE [Page_Section] = N'Lookup Values'

INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (@CHILDCARE_REQUEST_STATUS_PAGE
           ,@PAGE_SECTION)
GO

INSERT INTO [dbo].[cr_Childcare_Request_Statuses] 
		([Domain_ID],[Request_Status])
	     VALUES ( 1,'Pending'),
		        (1,'Conditionally Approved'),
				(1,'Approved'),
				(1,'Rejected');

GO


