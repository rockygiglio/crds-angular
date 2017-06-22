USE MinistryPlatform
GO

DECLARE @PageId int = 355
IF EXISTS(SELECT 1 FROM [dbo].[dp_Pages] WHERE Page_ID = @PageId)
BEGIN
	UPDATE [dbo].[dp_Pages]
	SET Default_Field_List = 'Contact_ID_Table.Display_Name 
,Contact_ID_Table.Nickname 
,Contact_ID_Table.First_Name 
,Contact_ID_Table_Contact_Status_ID_Table.Contact_Status 
,Participant_Type_ID_Table.Participant_Type ,Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.Congregation_Name 
,Contact_ID_Table.Date_of_Birth 
,Contact_ID_Table_Household_ID_Table.Home_Phone 
,Contact_ID_Table.Mobile_Phone 
,Contact_ID_Table_Household_ID_Table_Address_ID_Table.Address_Line_1 ,Contact_ID_Table_Household_ID_Table_Address_ID_Table.City ,Contact_ID_Table_Household_ID_Table_Address_ID_Table.[State/Region] ,Contact_ID_Table_Household_ID_Table_Address_ID_Table.Postal_Code 
,Contact_ID_Table.Email_Address 
,Contact_ID_Table_Household_ID_Table.Household_Name ,Contact_ID_Table_Household_Position_ID_Table.Household_Position 
,Participants.Participant_Start_Date AS [Participant_Start] 
,Participants.[Attendance_Start_Date]
,Participants.Participant_End_Date
,Group_Leader_Status_ID_Table.[Group_Leader_Status]'
	WHERE Page_ID = @PageId
END