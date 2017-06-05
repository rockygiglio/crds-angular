USE [MinistryPlatform]
GO

IF EXISTS(SELECT 1 FROM dp_Pages WHERE Page_ID = 337)
BEGIN
	UPDATE [dbo].[dp_Pages]
	SET Default_Field_List = 'Locations.Location_Name,Location_Type_ID_Table.Location_Type,Locations.Move_In_Date,Locations.Move_Out_Date'
	WHERE Page_ID = 337

END