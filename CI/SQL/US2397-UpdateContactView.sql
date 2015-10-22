USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
   SET [Field_List] = 'Contacts.Display_Name
, Contacts.[Contact_ID]
,Contacts.Nickname
,Contacts.First_Name
,Contact_Status_ID_Table.Contact_Status
,Household_ID_Table.Home_Phone
,Contacts.Mobile_Phone
,Household_ID_Table_Address_ID_Table.Address_Line_1
,Household_ID_Table_Address_ID_Table.Address_Line_2
,Household_ID_Table_Address_ID_Table.City
,Household_ID_Table_Address_ID_Table.[State/Region] AS State
,Household_ID_Table_Address_ID_Table.Postal_Code
,Contacts.Email_Address
,Convert(Varchar(12),Contacts.Date_of_Birth,101) AS Date_of_Birth
,Gender_ID_Table.Gender
,Marital_Status_ID_Table.Marital_Status
,Household_ID_Table_Congregation_ID_Table.Congregation_Name
,Household_ID_Table.Household_Name
,Household_Position_ID_Table.Household_Position
,Household_ID_Table_Address_ID_Table.Address_ID 
,Household_ID_Table_Congregation_ID_Table.Congregation_ID
,Household_ID_Table.Household_ID 
,Contacts.Anniversary_Date 
,Contacts.Employer_Name
,Household_ID_Table_Address_ID_Table.Foreign_Country 
,Gender_ID_Table.Gender_ID
,Contacts.Last_Name 
,Contacts.Middle_Name
,Contacts.Maiden_Name 
,Marital_Status_ID_Table.Marital_Status_ID 
,Mobile_Carrier_Table.[Phone_Carrier] AS [Mobile_Carrier] 
,Mobile_Carrier_Table.[Phone_Carrier_ID] AS [Mobile_Carrier_ID] 
,Contacts.[__Age] AS [Age]
,Contacts.[ID_Card] AS [ID_Card]
,Contacts.[Passport_Firstname] 
,Contacts.[Passport_Middlename] 
,Contacts.[Passport_Lastname] 
,Contacts.[Passport_Country] 
,Contacts.[Passport_Expiration] 
,Contacts.[Passport_Number]'

 WHERE Page_View_ID = 2129
GO

UPDATE [dbo].[dp_Pages]
   SET 
      [Default_Field_List] =N'Contacts.Contact_ID 
							 ,Contacts.First_Name
							 ,Contacts.Middle_Name
							 ,Contacts.Last_Name
							 ,Contacts.Nickname
							 ,Contacts.Email_Address
							 ,Contacts.Maiden_Name
							 ,Convert(Varchar(12)
							 ,Contacts.Date_of_Birth,101) AS Date_of_Birth
							 ,Marital_Status_ID_Table.Marital_Status_ID
							 ,Marital_Status_ID_Table.Marital_Status
							 ,Gender_ID_Table.Gender_ID 
							 ,Gender_ID_Table.Gender 
							 ,Contacts.Employer_Name 
							 ,Contacts.Anniversary_Date 
							 ,Contacts.Mobile_Phone 
							 ,Mobile_Carrier_Table.[Phone_Carrier] AS [Mobile_Carrier] 
							 ,Mobile_Carrier_Table.[Phone_Carrier_ID] AS [Mobile_Carrier_ID] 
							 ,Household_ID_Table.Household_ID 
							 ,Household_ID_Table_Address_ID_Table.Address_ID 
							 ,Household_ID_Table_Address_ID_Table.Address_Line_1 
							 ,Household_ID_Table_Address_ID_Table.Address_Line_2 
							 ,Household_ID_Table_Address_ID_Table.City 
							 ,Household_ID_Table_Address_ID_Table.County 
							 ,Household_ID_Table_Address_ID_Table.[State/Region] AS [State] 
							 ,Household_ID_Table_Address_ID_Table.Postal_Code 
							 ,Household_ID_Table_Address_ID_Table.Foreign_Country 
							 ,Household_ID_Table.Home_Phone ,Household_ID_Table_Congregation_ID_Table.Congregation_ID
							 ,Participant_Record_Table.Participant_ID 
							 ,Contacts.[__Age] AS [Age] 
							 ,Household_ID_Table.Household_Name 
							 ,Contacts.[Passport_Firstname] 
							 ,Contacts.[Passport_Middlename] 
							 ,Contacts.[Passport_Lastname] 
							 ,Contacts.[Passport_Country] 
							 ,Contacts.[Passport_Expiration] 
							 ,Contacts.[Passport_Number]'
      
 WHERE [Page_ID] = 474
GO 

