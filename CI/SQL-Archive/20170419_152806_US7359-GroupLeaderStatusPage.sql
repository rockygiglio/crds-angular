USE [MinistryPlatform]
GO

DECLARE @PageID int = 628
DECLARE @PageViewID int = 1120
DECLARE @PageSectionID INT = 4

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Pages] WHERE Page_ID = @PageID)
BEGIN
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
           (@PageID
		   ,'Group Leader Statuses'
           ,'Group Leader Status'
           ,'Statuses used for Group Leader Applications'
           ,10
           ,'cr_Group_Leader_Statuses'
           ,'Group_Leader_Status_ID'
           ,1
           ,'Group_Leader_Status, Sort_Order'
           ,'Group_Leader_Status'
           ,0)
	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = @PageViewID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
	INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause]
           ,[Order_By])
     VALUES
           (@PageViewID
		   ,'Group Leader Statuses'
           ,@PageID
           ,'Sorted list of Group Leaders Statuses'
           ,'Group_Leader_Status'
           ,'Group_Leader_Status_ID <> 0'
           ,'Sort_Order')
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END

UPDATE [dbo].[dp_Pages]
SET Pick_List_View = @PageViewID
WHERE Page_ID = @PageID

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Page_Section_Pages] WHERE Page_ID = @PageID AND Page_Section_ID = @PageSectionID )
BEGIN
	INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (@PageID
           ,@PageSectionID)
END