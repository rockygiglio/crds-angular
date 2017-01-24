USE MinistryPlatform
GO
DECLARE @genericuser INT = 9
UPDATE dbo.dp_Page_Views
SET User_ID = @genericuser
WHERE Page_View_ID IN (746,747,748)

GO