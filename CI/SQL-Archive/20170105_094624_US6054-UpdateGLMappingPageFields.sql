USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
   SET [Default_Field_List] = 'Program_ID_Table.[Program_Name]
	,Congregation_ID_Table.[Congregation_Name]
	,GL_Account_Mapping.[GL_Account]
	,GL_Account_Mapping.[Checkbook_ID]
	,GL_Account_Mapping.[Cash_Account]
	,GL_Account_Mapping.[Receivable_Account]
	,GL_Account_Mapping.[Distribution_Account]
	,GL_Account_Mapping.[Document_Type]
	,GL_Account_Mapping.[Customer_ID]
	,GL_Account_Mapping.[Scholarship_Expense_Account]'
 WHERE Page_ID = '504'
GO
