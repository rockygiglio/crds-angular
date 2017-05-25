USE MinistryPlatform
GO

DECLARE @REPORT_ID int = 306;
DECLARE @PAYMENT_PAGE_ID int = 359;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Reports] WHERE [Report_ID] = @REPORT_ID) 
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Reports] ON;
	INSERT INTO [dbo].[dp_Reports] (
		 [Report_ID]
		,[Report_Name]
		,[Description]
		,[Report_Path]
		,[Pass_Selected_Records]
		,[Pass_LinkTo_Records]
		,[On_Reports_Tab]
	) VALUES (
		 @REPORT_ID
		,N'Create Batch from Selected Payments'
		,N'This report will allow a trusted operator to select payments and instantly create a batch and a deposit record for those donations.'
		,N'/MPReports/Crossroads/CRDSPaymentsSelectedForBatch'
		,1
		,1
		,0
	)
	SET IDENTITY_INSERT [dbo].[dp_Reports] OFF;
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Report_Pages] WHERE [Report_ID] = @REPORT_ID AND [Page_ID] = @PAYMENT_PAGE_ID)
BEGIN
	INSERT INTO [dbo].[dp_Report_Pages] VALUES ( @REPORT_ID, @PAYMENT_PAGE_ID) 
END