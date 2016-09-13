use MinistryPlatform
GO

DECLARE @report_id int = 300;
DECLARE @page_id int;

DECLARE @reportpath NVARCHAR(500) = '/MPReports/Crossroads/CRDSAmigosExport'
DECLARE @reportname NVARCHAR(500) = 'Amigo''s Export'

SELECT @page_id = p.Page_ID FROM
	[dbo].[dp_Pages] p WHERE p.Display_Name = N'Events'

IF NOT EXISTS(SELECT * FROM [dbo].[dp_reports] WHERE report_id = @report_id)
BEGIN
	SET IDENTITY_INSERT dbo.dp_reports ON;  
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
			   (@report_id,
			    @reportname,
			    @reportname,
			    @reportpath,
			    1,
			    0,          
			    1)
	SET IDENTITY_INSERT dbo.dp_reports OFF; 
END
ELSE
BEGIN
	UPDATE [dbo].[dp_reports] SET
		Report_name = @reportname, Description = @reportname, Report_Path = @reportpath, Pass_Selected_Records = 1, Pass_LinkTo_Records = 0, On_Reports_Tab = 1
		WHERE Report_ID = @report_id
END

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

