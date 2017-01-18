USE MinistryPlatform
GO

DECLARE @PageViewId INT = 1117

IF EXISTS(SELECT * FROM dp_Page_Views where Page_View_ID = @PageViewId)
BEGIN
	UPDATE dp_Page_Views SET View_Clause = ' Contact_ID_Table_Household_ID_Table_Address_ID_Table.Postal_Code IS NOT NULL AND  Contact_ID_Table_Household_ID_Table_Address_ID_Table.Address_Line_1 IS NOT NULL AND  Contact_ID_Table_Household_ID_Table_Address_ID_Table.Address_Line_1 <> '''' AND Statement_Method_ID_Table.[Statement_Method_ID] = 1'
	     WHERE Page_View_ID = @PageViewId
END

GO
