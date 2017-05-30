USE MinistryPlatform
GO

DECLARE @ReportName NVARCHAR(64) = 'Childcare Detail'
DECLARE @ReportId INT = 293
DECLARE @PageId INT = 36 -- Childcare Requests

IF NOT EXISTS(SELECT * FROM dp_Reports WHERE Report_Name = @ReportName AND Report_ID = @ReportId)
BEGIN
	SET IDENTITY_INSERT dp_Reports ON
	INSERT INTO dp_Reports(Report_ID,
					   Report_Name,
					   Description,
					   Report_Path,
					   Pass_Selected_Records,
					   Pass_LinkTo_Records,
					   On_Reports_Tab)
			   VALUES(@ReportId,
			          @ReportName,
					  @ReportName,
					  '/MPReports/Crossroads/CRDSChildcareDetail',
					  0,
					  0,
					  1)
	SET IDENTITY_INSERT dp_Reports ON

	IF NOT EXISTS(SELECT * FROM dp_Report_Pages WHERE Report_ID = @ReportId AND Page_ID = @PageId)
	BEGIN
		INSERT INTO dp_Report_Pages(Report_ID,Page_ID) VALUES(@ReportId, @PageId)
	END
END

