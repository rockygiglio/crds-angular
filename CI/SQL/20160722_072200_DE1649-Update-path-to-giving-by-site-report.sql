USE MinistryPlatform
GO

IF EXISTS(SELECT * FROM dp_reports WHERE report_name='Giving By Site')
BEGIN
	DECLARE @ReportId INT;
	SELECT @ReportId=Report_ID FROM dp_reports WHERE report_name='Giving By Site'
	UPDATE dp_reports SET Report_Path='/MPReports/Crossroads/CRDSGivingBySite' WHERE Report_ID=@ReportId
END
GO