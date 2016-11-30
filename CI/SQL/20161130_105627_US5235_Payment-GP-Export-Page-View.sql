USE [MinistryPlatform]
GO

DECLARE @PageViewID int = 1112;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = @PageViewID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
	INSERT INTO [dbo].[dp_Page_Views]
			   ([Page_View_ID]
			   ,[View_Title]
			   ,[Page_ID]
			   ,[Field_List]
			   ,[View_Clause])
		 VALUES
			   (@PageViewID
			   ,'GP Export'
			   ,273 --All Payment Detail
			   ,'Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_ID] AS [Deposit ID]
	, Payment_ID_Table_Batch_ID_Table.[Batch_ID] AS [Batch ID]
	, Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AS [Program ID]
	, Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID] AS [Congregation ID]
	, (SELECT Document_Type FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID]) AS [Document_Type] 
	, Payment_ID_Table.[Payment_ID] AS [Payment ID]
	, Payment_ID_Table_Batch_ID_Table.[Batch_Name] AS [Batch Name]
	, Payment_ID_Table.[Payment_Date] AS [Payment Date]
	, Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_Date] AS [Deposit Date]
	, (SELECT Customer_ID FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID]) AS [Customer_ID]
	, Payment_ID_Table.[Payment_Total] AS [Payment Total]
	, (SELECT Checkbook_ID FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID]) AS [Checkbook_ID]
	, (SELECT Cash_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID]) AS [Cash_Account]
	, (SELECT Receivable_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID]) AS [Receivable_Account]
	, (SELECT Distribution_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID]) AS [Distribution_Account]
	, (SELECT Scholarship_Expense_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID]) AS [Scholarship_Expense_Account]
	, Payment_Detail.[Payment_Amount] AS [Payment Amount]
	, Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Exported] AS [Exported]
	, Payment_ID_Table_Payment_Type_ID_Table.[Payment_Type_ID] AS [Payment Type ID]
	, Payment_ID_Table.[Processor_Fee_Amount] AS [Processor Fee Amount]
	, Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_Amount] AS [Deposit Amount]'
			   ,'Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Exported] = 0'
	)
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END

