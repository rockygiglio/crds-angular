USE MinistryPlatform
GO

UPDATE dbo.dp_Page_Views
SET View_Clause = '1=1'
,Order_By = 'COALESCE(End_Date, ''12/31/9999'') DESC'
WHERE Page_View_ID = 2182
