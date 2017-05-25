USE [MinistryPlatform]
GO

Update dp_Page_Views
Set Field_list = 'Participant_ID_Table_Contact_ID_Table.[Display_Name] AS [Display Name] , Participant_ID_Table_Contact_ID_Table.[Nickname] , Participant_ID_Table_Contact_ID_Table.[Last_Name] , Preferred_Serving_Time_ID_Table.[Preferred_Serve_Time] , Group_ID_Table.[Group_Name] , Group_Role_ID_Table.[Role_Title] , Participant_ID_Table_Contact_ID_Table.[Email_Address] , Group_Participants.[Start_Date], Group_Participants.[End_Date] , Participant_ID_Table_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_Name] , Participant_ID_Table_Contact_ID_Table_Household_ID_Table.[Home_Phone] , Participant_ID_Table_Contact_ID_Table.[Mobile_Phone] , Participant_ID_Table_Contact_ID_Table_Household_ID_Table_Address_ID_Table.[Address_Line_1] , Participant_ID_Table_Contact_ID_Table_Household_ID_Table_Address_ID_Table.[Address_Line_2] , Participant_ID_Table_Contact_ID_Table_Household_ID_Table_Address_ID_Table.[City] , Participant_ID_Table_Contact_ID_Table_Household_ID_Table_Address_ID_Table.[State/Region] , Participant_ID_Table_Contact_ID_Table_Household_ID_Table_Address_ID_Table.[Postal_Code] , Group_ID_Table_Ministry_ID_Table.[Ministry_Name] , CASE WHEN Floor (datediff(DAY, ISNULL(Participant_ID_Table_Contact_ID_Table.Date_of_Birth, DATEADD(YEAR, -18, GETDATE())) , GETDATE()) / (365.23076923074 )) < 18 THEN ''Y'' ELSE ''N'' END AS StudentVolunteer'
where Page_View_ID = 92304
  

GO
