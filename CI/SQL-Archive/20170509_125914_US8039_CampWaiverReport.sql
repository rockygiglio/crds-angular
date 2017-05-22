USE [MinistryPlatform]
GO

DECLARE @ReportID int = 318;
DECLARE @PageID int = 605;
DECLARE @RoleID int = 1005;

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Reports] WHERE Report_ID = @ReportID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Reports] ON
	INSERT INTO [dbo].[dp_Reports]
           ([Report_ID]
		   ,[Report_Name]
           ,[Description]
           ,[Report_Path]
           ,[Pass_Selected_Records]
           ,[Pass_LinkTo_Records]
           ,[On_Reports_Tab]
           ,[Pass_Database_Connection])
     VALUES
           (@ReportID
		   ,'Waiver Report'
           ,'Waiver Report'
           ,'/MPReports/Crossroads/CRDS Summer Camp Waiver'
           ,0
           ,0
           ,0
           ,0)
	SET IDENTITY_INSERT [dbo].[dp_Reports] OFF
END

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Report_Pages] WHERE Report_ID = @ReportID)
BEGIN
	INSERT INTO [dbo].[dp_Report_Pages]
			(Report_ID
			,Page_ID)
	VALUES
			(@ReportID
			,@PageID)
END

IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Role_Reports] WHERE Report_ID = @ReportID AND Role_ID = @RoleID)
BEGIN
INSERT INTO [dbo].[dp_Role_Reports]
           ([Role_ID]
           ,[Report_ID]
           ,[Domain_ID])
     VALUES
           (@RoleID
           ,@ReportID
           ,1)
END

