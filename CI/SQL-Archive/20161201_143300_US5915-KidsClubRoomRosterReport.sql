
USE MinistryPlatform
GO

-- EXEC	@return_value = [dbo].[Get_Next_Available_ID]
--    @tableName = N'dp_Reports',
--    @description = N'US5915: Kids Club Room Roster Report'
DECLARE @ReportID INT = 304;

DECLARE @ReportName NVARCHAR(50) = 'Kids Club Event - Room Roster'
DECLARE @ReportDescription NVARCHAR(255) = 'Generate a Room roster for a Kids Club Event.'
DECLARE @ReportPath NVARCHAR(1000) = '/MPReports/Crossroads/CRDS Kids Club Event Room Roster'
DECLARE @PassSelected BIT = 1;
DECLARE @PassLinkTo BIT = 1;
DECLARE @OnReports BIT = 0;

DECLARE @ReportPage INT = 308; -- "Events" page
DECLARE @ReportRole INT = 112; -- "Kids Club Tools - CRDS" role
DECLARE @DomainID INT = 1;

-- Insert or update the report
IF NOT EXISTS (SELECT 1 FROM dp_Reports WHERE Report_ID = @ReportID)
BEGIN 
  PRINT 'Inserting ' + @ReportName;

  SET IDENTITY_INSERT dp_Reports ON;
  INSERT INTO dp_Reports (
    Report_ID
    , Report_Name
    , [Description]
    , Report_Path
    , Pass_Selected_Records
    , Pass_LinkTo_Records
    , On_Reports_Tab
  ) VALUES (
    @ReportID
    , @ReportName
    , @ReportDescription
    , @ReportPath
    , @PassSelected
    , @PassLinkTo
    , @OnReports
  );
  SET IDENTITY_INSERT dp_Reports OFF;
END
ELSE
BEGIN
  PRINT 'Updating ' + @ReportName
  UPDATE dp_Reports SET
    Report_Name = @ReportName
    , Description = @ReportDescription
    , Report_Path = @ReportPath
    , Pass_Selected_Records = @PassSelected
    , Pass_LinkTo_Records = @PassLinkTo
    , On_Reports_Tab = @OnReports
  WHERE Report_ID = @ReportID;
END;

-- Insert the Report Page, if needed
IF NOT EXISTS (SELECT 1 FROM dp_Report_Pages WHERE Report_ID = @ReportID AND Page_ID = @ReportPage)
BEGIN
  PRINT 'Inserting Report_Page for ' + @ReportName;
  INSERT INTO dp_Report_Pages (Report_ID, Page_ID) VALUES (@ReportID, @ReportPage);
END
ELSE
BEGIN
  PRINT 'Report_Page already exists for ' + @ReportName;
END;

-- Insert Role_Reports, if needed
IF NOT EXISTS (SELECT 1 FROM dp_Role_Reports WHERE Report_ID = @ReportID AND Role_ID = @ReportRole)
BEGIN
  PRINT 'Inserting Role_Report for ' + @ReportName;
  INSERT INTO dp_Role_Reports (Report_ID, Role_ID, Domain_ID) VALUES (@ReportID, @ReportRole, @DomainID);
END
ELSE
BEGIN
  PRINT 'Role_Report already exists for ' + @ReportName;
END;
