USE [MinistryPlatform]
GO

IF EXISTS(SELECT 1 FROM dp_Pages WHERE Page_ID = 11)
BEGIN
	UPDATE [dbo].[dp_Pages]
	SET Default_Field_List = 'Initiative_Name, Program_ID_Table.Program_Name, cr_Initiatives.Start_Date, cr_Initiatives.End_Date, Leader_Signup_Start_Date, Leader_Signup_End_Date, Volunteer_Signup_Start_Date, Volunteer_Signup_End_Date'
	WHERE Page_ID = 11

END