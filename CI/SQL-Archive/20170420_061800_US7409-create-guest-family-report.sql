USE MinistryPlatform
GO

GO

DECLARE @report_id INT;
DECLARE @page_id INT;

SELECT @page_id = p.Page_ID FROM
	[dbo].[dp_Pages] p WHERE p.Display_Name = N'Events'

SET IDENTITY_INSERT [dbo].[dp_Reports] ON

IF NOT EXISTS(SELECT * FROM [dbo].[dp_reports] WHERE Report_Path = '/MPReports/Crossroads/CRDS Checkin Guest Family')
BEGIN
	INSERT INTO [dbo].[dp_reports]
			   ([Report_ID],
			    [Report_Name],
				[Description],
				[Report_Path],
				[Pass_Selected_Records],
				[Pass_LinkTo_Records],
				[On_Reports_Tab]
			   )
		 VALUES
			   (317,
			   N'Event Guest Report'
			   ,N'Event Guest Report'
			   ,N'/MPReports/Crossroads/CRDS Checkin Guest Family'
			   ,0
			   ,0          
			   ,1)
END

SET IDENTITY_INSERT [dbo].[dp_Reports] OFF

SELECT @report_id = r.Report_ID FROM
	[dp_reports] r WHERE r.Report_Path = N'/MPReports/Crossroads/CRDS Checkin Guest Family'

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Report_Pages] WHERE Report_ID = @report_id AND Page_ID = @page_id )
BEGIN
	INSERT INTO [dbo].[dp_Report_Pages]
		(
			[Report_ID],
			[Page_ID]
		)
		VALUES
		(
			@report_id,
			@page_id
		)
END
GO