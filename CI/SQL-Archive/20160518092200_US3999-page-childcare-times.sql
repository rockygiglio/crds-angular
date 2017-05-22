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
           (37
		   ,'Childcare Preferred Times'
           ,'Childcare Preferred Times'
           ,'Childcare Preferred Times' 
           ,70
           ,'cr_Childcare_Preferred_Times'
           ,'Childcare_Preferred_Time_ID'
		   ,NULL
           ,'Congregation_ID_Table.[Congregation_Name] AS [Congregation Name]
				, [Childcare_Start_Time] AS [Childcare Start Time] 
				, [Childcare_End_Time] AS [Childcare End Time]  
				, Childcare_Day_ID_Table.[Meeting_Day] AS [Childcare Day]'
           ,'Childcare_Preferred_Time_ID'
           ,1);

		   SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO

DECLARE @CHILDCARE_REQUEST_STATUS_PAGE int;
DECLARE @PAGE_SECTION int;

SELECT @CHILDCARE_REQUEST_STATUS_PAGE = [Page_ID] FROM  [dbo].[dp_Pages] p
	WHERE p.Display_Name = N'Childcare Preferred Times'

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


