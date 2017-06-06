USE MinistryPlatform
GO

DECLARE @NewPageViewId INT = 1117;
DECLARE @PageId INT = 299;

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] where Page_View_ID = @NewPageViewId) 
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

	INSERT INTO [dbo].[dp_Page_Views](Page_View_ID, View_Title, Page_ID, Description, Field_List, View_Clause)
		VALUES (@NewPageViewId,
		       'Contribution Statements',
			   @PageId,
			   'Selection View for Year End Search',
			   'Contact_ID_Table.Display_Name ,Contact_ID_Table.Nickname ,Contact_ID_Table.First_Name + ISNULL('' '' + Contact_ID_Table.Middle_Name,'''') AS First_Middle ,Contact_ID_Table_Household_Position_ID_Table.Household_Position ,(SELECT Max(First_Name) FROM Contacts C WHERE C.Household_ID = Contact_ID_Table.Household_ID AND Contact_ID_Table.Household_Position_ID = 1 AND Contact_ID_Table.Household_Position_ID = C.Household_Position_ID AND C.Contact_ID <> Donors.Contact_ID) AS Spouse_Name ,Donors.Envelope_No ,Contact_ID_Table_Household_ID_Table_Address_ID_Table.Address_Line_1 ,Contact_ID_Table_Household_ID_Table_Address_ID_Table.City ,Contact_ID_Table_Household_ID_Table_Address_ID_Table.[State/Region] ,Contact_ID_Table_Household_ID_Table_Address_ID_Table.Postal_Code ,Donors.Donor_ID AS DonorID ,Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.Congregation_Name',
			   'Contact_ID_Table_Household_ID_Table_Address_ID_Table.Postal_Code IS NOT NULL AND Statement_Method_ID_Table.Statement_Method_ID = 1')

	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
GO