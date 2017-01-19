USE MinistryPlatform
GO

DECLARE @REPORT_ID int = 311
DECLARE @HOUSEHOLD_PAGE_ID int = 327;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Reports] WHERE [Report_ID] = @REPORT_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Reports] ON
	INSERT INTO [dbo].[dp_Reports] (
		 [Report_ID]
		,[Report_Name]
		,[Description]:
		,[Report_Path]
		,[Pass_Selected_Records]
		,[Pass_LinkTo_Records]
		,[On_Reports_Tab]
	) VALUES (
		 @REPORT_ID
		,N'Check In: Duplicate Phone Number'
        ,N'Finds duplicate phone numbers'
		,N'/MPReports/Crossroads/CRDS_Duplicate_Phone_Numbers'
		,1
		,0
		,1 
	)
	SET IDENTITY_INSERT [dbo].[dp_Reports] OFF
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Report_Pages] WHERE [Report_ID] = @REPORT_ID AND [Page_ID] = 327)
BEGIN
	INSERT INTO [dbo].[dp_Report_Pages] VALUES (@REPORT_ID, @HOUSEHOLD_PAGE_ID)
END