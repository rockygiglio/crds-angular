use MinistryPlatform
GO

DECLARE @report_id int;
DECLARE @page_id int;

SELECT @page_id = p.Page_ID FROM
	[dbo].[dp_Pages] p WHERE p.Display_Name = N'Donations'

IF NOT EXISTS(SELECT * FROM [dbo].[dp_reports] WHERE Report_Path = '/MPReports/CRDS New Givers')
BEGIN
	INSERT INTO [dbo].[dp_reports]
			   ([Report_Name],
				[Description],
				[Report_Path],
				[Pass_Selected_Records],
				[Pass_LinkTo_Records],
				[On_Reports_Tab]
			   )
		 VALUES
			   (N'New Givers'
			   ,N'New Givers'
			   ,N'/MPReports/CRDS New Givers'
			   ,1
			   ,0          
			   ,1)
END

SELECT @report_id = r.Report_ID FROM
	[dp_reports] r WHERE r.Report_Path = N'/MPReports/CRDS New Givers'

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
